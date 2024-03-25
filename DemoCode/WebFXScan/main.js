$(document).ready(function () {
  $("#scan").on("click", async function () {
    try {
      $(".loading-mask").removeClass("hide");
      const { result, message, data, error } = await MyScan.scan();
      if (result) {
        $(".loading-mask").addClass("hide");
        data.map((file) => {
          const { fileName, base64 } = file;
          $("#img-zone").attr("src", base64);
          $("#filename").text(fileName);
        });
      } else {
        console.log(error);
      }
    } catch (e) {
      console.warn(e);
      $(".loading-mask").addClass("hide");
      const { error = "unknown" } = e;
      alert(`Scan error: ${error}`);
    }
  });

  async function init() {
    try {
      await MyScan.connect({ ip: "127.0.0.1", port: "17778" });
      await MyScan.init();

      const { data: optionData } = await MyScan.getDeviceList();
      const { options, groupOptions } = optionData;
      // check scanner is exist
      if (options.length < 1) {
        throw new Error({ error: "Scanner not detected." });
      }
      // default first device.
      const { deviceName = "", source = {} } = options[0];
      const { value: sourceAry = [] } = source;
      if (sourceAry.length < 1) {
        throw new Error({ error: "Scanner model identification failed." });
      }
      const scannerConfig = {
        deviceName: deviceName, // first device
        source: sourceAry[0], // paper orientation
        paperSize: "A4",
        resolution: 300, // dpi
        mode: "color",
        brightness: 0,
        contrast: 0,
        quality: 75,
        swcrop: true, // auto crop
        swdeskew: true, // auto deskew
      };

      await MyScan.setScanner(scannerConfig);

      $("#status").removeClass("connect-color").addClass("success-color");
      $("#status").text("Connection successful.");
      $("#scan").prop("disabled", false);
    } catch (e) {
      console.warn(e);
      const { error = "unknown" } = e;
      $("#status").removeClass("connect-color").addClass("failure-color");
      $("#status").text("Connection failed");
      alert(`Scanner initialization error: ${error}`);
    }
  }

  const MyScan = new WebFxScan();
  init();
});
