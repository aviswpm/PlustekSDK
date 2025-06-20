$(document).ready(function () {
  const isDebug = false;
  /*** Events ***/
  // send scan command to server
  $("#scan").on("click", async function () {
    try {
      view.displayLoadingMask(true);
      const { result, message, data, error } = await MyScan.scan();
      if (result) {
        imageAction.clear();
        view.displayLoadingMask(false);
        data.map((file) => {
          imageAction.addImage(file);
        });
        imageAction.updateTotal(data.length);
        imageAction.to(1);
      } else {
        console.log(error);
      }
    } catch (e) {
      console.warn(e);
      view.displayLoadingMask(false);
      const { error = "unknown" } = e;
      alert(`Scan error: ${error}`);
    }
  });

  // form component change event
  $("#device-name").on("change", function (e) {
    updateServerProperty({ deviceName: this.value });
  });

  $("#source").on("change", function (e) {
    updateServerProperty({ source: this.value });
  });

  $("#recognizer-type").on("change", function (e) {
    // last opt equal enable custom input
    if (
      this.value !==
      globalParam.recognizeTypeOpts[globalParam.recognizeTypeOpts.length - 1]
    ) {
      updateServerProperty({ recognizeType: this.value });
      view.displayCustomRecognizeTypeInput(false);
    } else {
      const otherValue = "";
      $("#custom-recognize-type input").val(otherValue);
      updateServerProperty({ recognizeType: otherValue });
      view.displayCustomRecognizeTypeInput(true);
    }
  });

  $("#custom-recognize-type input").on("change", function (e) {
    updateServerProperty({ recognizeType: $(this).val() });
  });

  // show message window
  $("#message-container").on("click", ".custom-accordion-button", function (e) {
    const id = $(this).data("id");
    const logDetail = logger.getLog(id);
    const logDetailObj = JSON.parse(logDetail);
    view.displayOcrTextWindow("Message", logDetailObj);
    e.stopPropagation();
  });

  // show recognize window
  $("#show-recognize").on("click", function (e) {
    const id = $(this).data("id");
    const imageObj = proxyImageData.imageCache[proxyImageData.index - 1];
    const { ocrText = "" } = imageObj;
    view.displayOcrTextWindow("Recognize Data", ocrText);
    e.stopPropagation();
  });

  // show set scanner window
  $("#set-scanner").on("click", function (e) {
    const currentConfigString = JSON.stringify(
      globalParam.scannerConfig,
      null,
      2
    );

    view.displaySetScannerWindow(currentConfigString);
    e.stopPropagation();
  });

  // set scanner from window
  $("#set-scanner-window .custom-set-scanner-window-button").on(
    "click",
    async function (e) {
      let propertiesObj;
      try {
        propertiesObj = JSON.parse($("#set-scanner-window textarea").val());
      } catch (e) {
        console.warn(e);
        alert(`JSON parser error: ${e.message}`);
        return;
      }

      try {
        await updateServerProperty(propertiesObj, true);
        $("#set-scanner-window").addClass("d-none");
      } catch (e) {
        console.warn(e);
        const { error = "unknown" } = e;
        alert(`Set scanner error: ${error}`);
      }
    }
  );

  // clear all message
  $("#clear-message").on("click", function (e) {
    logger.clear();
    view.clearMessage();
  });

  // clear all message
  $("#test").on("click", async function (e) {
    const testData = await MyScan.getPaperStatus();
    console.log(testData);
  });

  // close ocr text modal
  $("#ocrtext-window .custom-modal-mask, #ocrtext-window .custom-close-btn").on(
    "click",
    function (e) {
      $("#ocrtext-window").addClass("d-none");
    }
  );

  // close set scanner modal
  $(
    "#set-scanner-window .custom-modal-mask, #set-scanner-window .custom-close-btn"
  ).on("click", function (e) {
    $("#set-scanner-window").addClass("d-none");
  });

  // change page
  $("#page-prev").on("click", function (e) {
    imageAction.prev();
  });
  $("#page-next").on("click", function (e) {
    imageAction.next();
  });
  $("#current-page-input").on("change", function (e) {
    imageAction.to($(this).val());
  });

  // vtm 300 eject paper
  $("#eject-back").on("click", async function (e) {
    await MyScan.ejectPaper({ isBackward: true });
  });
  $("#eject-front").on("click", async function (e) {
    await MyScan.ejectPaper({ isBackward: false });
  });

  /*** Image proxy ***/
  const imageData = {
    imageCache: [],
    index: 0,
    total: 0,
  };

  // event -> iamgeAction -> proxy -> UI
  let proxyImageHandler = {
    set: function (obj, prop, value) {
      if (prop === "index") {
        // show pic if index change
        const pageIndex = parseInt(value);
        if (isNaN(pageIndex) || pageIndex < 1 || pageIndex > obj.total) {
          return false;
        } else {
          obj[prop] = pageIndex;
          view.showPic(obj.imageCache[pageIndex - 1]);
          view.updatePageIndex(pageIndex);
          return true;
        }
      } else if (prop === "imageCache" && Array.isArray(value)) {
        obj[prop] = value;
        obj.total = value.length;
        view.updatePageTotal(value.length);
        return true;
      } else if (prop === "total") {
        const pageTotal = parseInt(value);
        view.updatePageTotal(pageTotal);
        obj[prop] = pageTotal;
        return true;
      } else if (prop === "newImage") {
        obj.imageCache.push(value);
        obj.total = obj.imageCache.length;
        obj.index = obj.imageCache.length;
        view.updatePageTotal(obj.imageCache.length);
        view.showPic(obj.imageCache[obj.imageCache.length - 1]);
        view.updatePageIndex(obj.imageCache.length);
        return true;
      }

      return false;
    },
    get: function (obj, prop) {
      // if prop exist
      if (prop in obj) {
        return Reflect.get(obj, prop);
      } else {
        return `Property ${prop} does not exist.`;
      }
    },
  };
  let proxyImageData = new Proxy(imageData, proxyImageHandler);

  let imageAction = {
    prev: () => {
      proxyImageData.index = proxyImageData.index - 1;
    },
    next: () => {
      proxyImageData.index = proxyImageData.index + 1;
    },
    to: (page) => {
      proxyImageData.index = page;
    },
    updateTotal: (total) => {
      proxyImageData.total = total;
    },
    addImage: (fileObj) => {
      proxyImageData.newImage = fileObj;
    },
    clear: () => {
      proxyImageData.imageCache = [];
      proxyImageData.total = 0;
      proxyImageData.index = 0;
    },
  };

  /*** Parameters & Data ***/
  const globalParam = {
    scannerConfig: {
      resolution: 300,
      mode: "color",
      brightness: 0,
      contrast: 0,
      quality: 75,
    },
    recognizeTypeOpts: [
      "twid",
      "cnid",
      "egid",
      "maid",
      "vnid",
      "idid",
      "hkid",
      "svid",
      "insurance",
      "passport",
      "fulltext",
      "barcode",
      "receipt",
      "gridmark",
      "passport-loose",
      "form",
      "auto",
      "cn-invoice",
      "twrc",
      "twpass",
      "usdl",
      "omr",
      "omradvanced",
      "other",
    ],
  };

  /*** logger ***/
  const logger = {
    counter: 0,
    history: [],
    addLog: (log) => {
      logger.history.push(log);
      logger.counter++;
    },
    getLog: (id) => {
      return logger.history[id];
    },
    getCounter: () => {
      return logger.counter;
    },
    clear: () => {
      logger.counter = 0;
      logger.history = [];
    },
  };

  /*** UI functions ***/
  const view = {
    debugMode(isDebug) {
      if (isDebug) {
        $("#test").removeClass("d-none");
      }
    },
    showPic(file) {
      const { fileName, base64, ocrText } = file;
      $("#sample-bg").addClass("d-none");
      $("#img-zone").attr("src", base64);
      if (ocrText === "") {
        $(".custom-button-recognize").attr("disabled", true);
      } else {
        $(".custom-button-recognize").attr("disabled", false);
      }
    },
    addLogEntry(log, msgType) {
      // cahce log
      const logId = logger.getCounter();
      logger.addLog(log);

      // message record template
      const logTitle = log.substring(0, 100);
      const isSendType = msgType === "up";
      const accordionItem = `
      <div class="accordion-item">
        <h2 class="accordion-header" id="heading${logId}">
          <div
            class="${
              isSendType
                ? "custom-message-up-bg-color"
                : "custom-message-down-bg-color"
            } custom-accordion-button p-1 ps-2"
            type="button"
            data-id="${logId}"
          >
            <div class="${
              isSendType ? "custom-arrow-up" : "custom-arrow-down"
            }">
              ${isSendType ? "↑" : "↓"}
            </div>
            <div class="custom-text-line-2 custom-font-size-mid">
              ${logTitle}
            </div>
          </div>
        </h2>
      </div>`;
      $("#log-container").append(accordionItem);

      // scroll to bottom
      $("#message-container").scrollTop(
        $("#message-container")[0].scrollHeight
      );
    },
    disableForm(isDisabled) {
      $("#main-form")
        .find("input, select, textarea, button")
        .attr("disabled", isDisabled);
    },
    displayOcrTextWindow(title = "", contentObj) {
      $("#ocrtext-window .custom-window-title").text(title);
      $("#ocrtext-window .custom-window-content").jsonViewer(contentObj, {
        collapsed: true,
        rootCollapsable: false,
      });
      $("#ocrtext-window").removeClass("d-none");
    },
    displaySetScannerWindow(currentConfigString) {
      $("#set-scanner-window textarea").val(currentConfigString);
      $("#set-scanner-window").removeClass("d-none");
    },
    displayCustomRecognizeTypeInput(isVisible) {
      if (isVisible) {
        $("#custom-recognize-type").removeClass("d-none");
      } else {
        $("#custom-recognize-type").addClass("d-none");
      }
    },
    displayLoadingMask(isVisible) {
      if (isVisible) {
        $(".custom-loading-mask").removeClass("d-none");
      } else {
        $(".custom-loading-mask").addClass("d-none");
      }
    },
    setRecognizeTypeOpts(opts) {
      $("#recognizer-type").empty();
      // insert [none] opt
      $("#recognizer-type").append(`<option value="">none</option>`);
      // insert avalible opt
      opts.forEach((recognizeType) => {
        $("#recognizer-type").append(
          `<option value="${recognizeType}">${recognizeType}</option>`
        );
      });
    },
    setDeviceOpts(deviceObjs) {
      $("#device-name").empty();
      deviceObjs.forEach((deviceObj) => {
        const { deviceName } = deviceObj;
        $("#device-name").append(
          `<option value="${deviceName}">${deviceName}</option>`
        );
      });
    },
    setSourceOpts(sourceAry) {
      $("#source").empty();
      sourceAry.forEach((source) => {
        $("#source").append(`<option value="${source}">${source}</option>`);
      });
    },
    clearMessage() {
      $("#log-container").empty();
    },
    updateVersion(version) {
      $("#version").text(version);
    },
    updatePageIndex(index) {
      $("#current-page-input").val(index);
    },
    updatePageTotal(total) {
      $("#total-pages").text(total);
    },
  };

  /*** General function ***/
  async function init() {
    try {
      view.debugMode(isDebug);
      // display information
      view.disableForm(true);
      // update recognize select
      view.setRecognizeTypeOpts(globalParam.recognizeTypeOpts);
      const version = await MyScan.getVersion();
      view.updateVersion(version);

      // connect server
      await MyScan.connect({ ip: "127.0.0.1", port: "17778" });
      await MyScan.setAutoScanCallback({
        callback: (file, errCode) => {
          if(errCode === 0) {
            imageAction.addImage(file);
          }
          view.displayLoadingMask(false);
        },
      });
      // for some device like A380 plus
      await MyScan.setBeforeAutoScanCallback({
        callback: () => {
          view.displayLoadingMask(true);
        },
      });
      // await MyScan.setSocketMsgCollector({ callback: view.addLogEntry });
      await MyScan.init();

      const { data: optionData } = await MyScan.getDeviceList();
      const { options } = optionData;

      // check is any scanner exist
      if (options.length < 1) {
        throw new Error("Scanner not detected.");
      }
      // select first device as default.
      const { deviceName = "", source = {} } = options[0];
      const { value: sourceAry = [] } = source;
      if (sourceAry.length < 1) {
        throw new Error("Scanner model identification failed.");
      }

      // update device select
      view.setDeviceOpts(options);
      view.setSourceOpts(sourceAry);

      // send scanner properties to server
      const initRecognizerType = $("#recognizer-type").val();
      await updateServerProperty({
        deviceName: deviceName, // first device
        source: sourceAry[0],
        recognizeType: initRecognizerType,
      });

      // enable form when ready
      view.disableForm(false);
    } catch (e) {
      console.warn(e);
      const { message = "unknown", error = 0 } = e;

      alert(
        `Scanner initialization error: ${(msg = error === 0 ? message : error)}`
      );
    }
  }

  async function updateServerProperty(newConfig, isOverwrite = false) {
    // remove recognizeType if empty
    globalParam.scannerConfig = isOverwrite
      ? { ...newConfig }
      : { ...globalParam.scannerConfig, ...newConfig };
    const { recognizeType = "", ...otherParam } = globalParam.scannerConfig;
    const finalScannerConfig =
      recognizeType === "" ? otherParam : globalParam.scannerConfig;
    try {
      await MyScan.setScanner(finalScannerConfig);
    } catch (e) {
      console.warn(e);
      const { message = "unknown", error = 0 } = e;

      alert(
        `Scanner initialization error: ${(msg = error === 0 ? message : error)}`
      );
    }
  }

  // wrap lib to histroy log
  function serialize(obj) {
    if (typeof obj === "function") {
      return obj.toString();
    }
    if (obj === null || typeof obj !== "object") {
      return obj;
    }
    if (Array.isArray(obj)) {
      return obj.map((item) => serialize(item));
    }
    const serializedObj = {};
    for (const key in obj) {
      if (obj.hasOwnProperty(key)) {
        serializedObj[key] = serialize(obj[key]);
      }
    }
    return serializedObj;
  }

  function wrapLib(instance) {
    Object.getOwnPropertyNames(Object.getPrototypeOf(instance))
      .filter(
        (prop) => typeof instance[prop] === "function" && prop !== "constructor"
      )
      .forEach((methodName) => wrapLibHanlder(instance, methodName));
  }

  function wrapLibHanlder(instance, methodName) {
    const originalMethod = instance[methodName];
    instance[methodName] = async function (...args) {
      const log = { API: methodName, args: serialize(args) };
      view.addLogEntry(JSON.stringify(log), "up");
      let result = {};
      try {
        result = await originalMethod.apply(this, args);
      } catch (e) {
        result = e;
        const resultLog = { API: methodName, return: serialize(result) };
        view.addLogEntry(JSON.stringify(resultLog), "down");
        throw e;
      }
      const resultLog = { API: methodName, return: serialize(result) };
      view.addLogEntry(JSON.stringify(resultLog), "down");
      return result;
    };
  }

  /*** Main ***/
  const MyScan = new WebFxScan();
  wrapLib(MyScan);
  init();
});
