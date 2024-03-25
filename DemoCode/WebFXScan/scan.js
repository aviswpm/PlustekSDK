/***
Architecture:
  WebFxScan -> WebFxScanServer
      |         |---> WebFxScanUtility
      |---------|---> WebFxScanScanner

Flow:
  WebFxScan's method -> 
  WebFxScanServer's async method -> notify -> set promise to specified apiList -> wait for response -> callback 
***/

/* Scanner parameter table */
class WebFxScanScanner {
  // scanner model
  static devices = {
    291: "type2",
    297: "type2",
    550: "type7",
    647: "type1",
    691: "type1",
    692: "type1",
    695: "type1",
    696: "type1",
    698: "type1",
    776: "type9",
    1180: "type6",
    1901: "type3",
    1902: "type1",
    1903: "type1",
    1912: "type1",
    1916: "type1",
    "671U": "type1",
    "0C51": "type3",
    "69KU": "type1",
    "696U": "type1",
    "691U": "type1",
    K14U: "type1",
    "647U": "type1",
    K11U: "type1",
    "65MU": "type1",
    K1AU: "type1",
    K13U: "type1",
    "692U": "type1",
    "65PU": "type1",
    K19U: "type1",
    J11U: "type1",
    "695U": "type1",
    K1FU: "type1",
    "65JU": "type1",
    "698U": "type1",
    "65QU": "type1",
    "65FU": "type1",
    "271U": "type1",
    Q28U: "type1",
    "638U": "type1",
    "632U": "type1",
    "251U": "type1",
    "65Q": "type1",
    K13: "type1",
    "65M": "type1",
    "65P": "type1",
    J11: "type1",
    "65J": "type1",
    "69D": "type1",
    "69K": "type1",
    K14: "type1",
    K19: "type1",
    K11: "type1",
    K1F: "type1",
    "29A": "type2",
    "69B": "type1",
    K54: "type1",
    "677U": "type3",
    "656U": "type1",
    "69DU": "type1",
    "29AU": "type2",
    "69BU": "type1",
    K54U: "type1",
    "684U": "type1",
    K1GU: "type1",
    K1HU: "type1",
    K7AU: "type1",
    A61: "type4",
    A62: "type4",
    A63: "type4",
    "6C1U": "type1",
    "7C1U": "type5",
    "67JU": "type3",
    "1680H": "type6",
    A320L: "type6",
    "2B3U": "type6",
    "7CCU": "type5",
    K77U: "type1",
    K76U: "type1",
    K7KU: "type1",
    K71U: "type1",
    K7BU: "type1",
    "29P": "type2",
    K81U: "type1",
    "PT2160Device-1": "type1", // linux specila spec
    K88U: "type1",
    K1KU: "type1",
    "25BU": "type6",
    K83U: "type1",
    K7VU: "type1",
    "2B6U": "type6",
    "2C2U": "type6",
    "2D2U": "type6",
    K93U: "type8",
    "2BDU": "type6",
    "2BHU": "type6",
    "2D4U": "type6",
    "2D5U": "type6",
    "67HU": "type3",
    "6C7U": "type3",
    "6C9U": "type3",
    K5GU: "type1",
    K5KU: "type1",
    K78U: "type1",
    K95U: "type1",
    K96U: "type1",
    K97U: "type1",
    A64: "type4",
    M11U: "type10",
    M12U: "type10",
    M21U_M11U: "type10",
  };

  // support for old webFXScan version
  static scannerOption = {
    type1: {
      source: {
        value: ["ADF-Back", "ADF-Duplex", "ADF-Front"],
        type: "list",
      },
      paperSize: {
        value: ["A4", "A5", "A6", "Letter", "B5", "B6"],
        type: "list",
      },
      resolution: {
        value: [100, 150, 200, 250, 300],
        type: "list",
      },
      mode: { value: ["lineart", "gray", "color"], type: "list" },
    },
    type2: {
      source: {
        value: ["ADF-Back", "ADF-Duplex", "ADF-Front"],
        type: "list",
      },
      paperSize: {
        value: ["A3", "A4", "A5", "A6", "Letter", "B5", "B6"],
        type: "list",
      },
      resolution: {
        value: [100, 150, 200, 250, 300],
        type: "list",
      },
      mode: { value: ["lineart", "gray", "color"], type: "list" },
    },
    type3: {
      source: {
        value: ["ADF-Back", "ADF-Duplex", "ADF-Front", "Flatbed"],
        type: "list",
      },
      paperSize: {
        value: ["A4", "A5", "A6", "Letter", "B5", "B6"],
        type: "list",
      },
      resolution: {
        value: [100, 150, 200, 250, 300],
        type: "list",
      },
      mode: { value: ["lineart", "gray", "color"], type: "list" },
    },
    type4: {
      source: { value: ["Camera"], type: "list" },
      paperSize: {
        value: ["2592x1944", "2048x1536"],
        type: "list",
      },
      resolution: { value: [300], type: "list" },
      mode: { value: ["color"], type: "list" },
    },
    type5: {
      source: {
        value: ["SHEEFED-Back", "SHEEFED-Duplex", "SHEEFED-Front"],
        type: "list",
      },
      paperSize: {
        value: ["A6"],
        type: "list",
      },
      resolution: {
        value: [100, 150, 200, 250, 300],
        type: "list",
      },
      mode: { value: ["lineart", "gray", "color"], type: "list" },
    },
    type6: {
      source: { value: ["Flatbed"], type: "list" },
      paperSize: {
        value: ["A3", "A4", "A5", "B5", "A4_Landscape", "A5_Landscape"],
        type: "list",
      },
      resolution: {
        value: [150, 200, 300, 600, 1200],
        type: "list",
      },
      mode: { value: ["lineart", "gray", "color"], type: "list" },
    },
    type7: {
      source: { value: ["Flatbed"], type: "list" },
      paperSize: {
        value: ["none"],
        type: "none",
      },
      resolution: {
        value: [150, 200, 300, 600, 1200],
        type: "list",
      },
      mode: { value: ["lineart", "gray", "color"], type: "list" },
    },
    type8: {
      source: {
        value: ["ADF-Back", "ADF-Duplex", "ADF-Front"],
        type: "list",
      },
      paperSize: {
        value: [
          "A4",
          "A5",
          "A6",
          "Letter",
          "B5",
          "B6",
          "long_paper_50",
          "long_paper_100",
          "long_paper_150",
          "long_paper_200",
        ],
        type: "list",
      },
      resolution: {
        value: [100, 150, 200, 250, 300],
        type: "list",
      },
      mode: { value: ["lineart", "gray", "color"], type: "list" },
    },
    type9: {
      source: {
        value: ["Sheetfed-Front", "Sheetfed-Back", "Sheetfed-Duplex"],
        type: "list",
      },
      paperSize: {
        value: ["A4", "A5", "A6", "B5", "B6"],
        type: "list",
      },
      resolution: {
        value: [100, 150, 200, 250, 300],
        type: "list",
      },
      mode: { value: ["lineart", "gray", "color"], type: "list" },
    },
    type10: {
      source: {
        value: ["Sheetfed-Front", "Sheetfed-Back", "Sheetfed-Duplex"],
        type: "list",
      },
      paperSize: {
        value: ["A4", "A5", "A6", "B5", "B6"],
        type: "list",
      },
      resolution: {
        value: [100, 150, 200, 250, 300],
        type: "list",
      },
      mode: { value: ["lineart", "gray", "color"], type: "list" },
    },
  };
  // available general option
  static scanOption = {
    brightness: { value: [-100, 100], type: "range" },
    contrast: { value: [-100, 100], type: "range" },
    quality: { value: [0, 100], type: "range" },
  };
  // available IP option
  static imageOption = {
    swcrop: { value: [true, false], type: "switch" },
    swdeskew: { value: [true, false], type: "switch" },
    frontEject: { value: [true, false], type: "switch" },
    manualEject: { value: [true, false], type: "switch" },
    removeBlankPage: { value: [true, false], type: "switch" },
    removeBlackEdges: { value: [true, false], type: "switch" },
    denoise: { value: [true, false], type: "switch" },
    multifeedDetect: { value: [true, false], type: "switch" },
    borderFill: { value: [true, false], type: "switch" },
    removePunchHole: { value: [true, false], type: "switch" },
    redTypeEnhance: { value: [true, false], type: "switch" },
  };
  // available ocr option
  static ocrOption = {
    recognizeType: { value: ["fulltext", "barcode", "none"], type: "list" },
    recognizeLang: {
      value: [
        "en",
        "zh-tw+en",
        "zh-cn+en",
        "de+en",
        "jp",
        "kp",
        "es",
        "vn+en",
        "ae+en",
      ],
      type: "list",
    },
    fullTextType: { value: ["txt", "pdf", "odf"], type: "list" },
  };
  // export option
  static exportOption = {
    savePath: { value: "", type: "input" },
  };
  // Parameter conversion table between the WebFxScan interface and the WebFxScanServer interface. For detailed explanations, please refer to the documentation page.
  static interfaceTable = {
    deviceName: { type: "string", value: "device-name" },
    source: { type: "string", value: "source" },
    paperSize: { type: "string", value: "paper-size" },
    td2: { type: "boolean", value: "td2" },
    recognizeType: { type: "string", value: "recognize-type" },
    recognizeLang: { type: "string", value: "recognize-lang" },
    fullTextType: { type: "string", value: "fulltext-type" },
    omrParm: { type: "string", value: "omr-parm" },
    autoScan: { type: "boolean", value: "autoscan" },
    apAutoScan: { type: "boolean", value: "apautoscan" },
    softwareSensor: { type: "boolean", value: "softwaresensor" },
    autoRotate: { type: "boolean", value: "autorotate" },
    rotate: { type: "number", value: "rotate" },
    duplexBackFlip: { type: "boolean", value: "duplexbackflip" },
    duplexMergeType: { type: "number", value: "duplexmergetype" },
    autoEnhance: { type: "boolean", value: "autoenhance" },
    mode: { type: "string", value: "mode" },
    convertImgMode: { type: "string", value: "convertimgmode" },
    sensorAlign: { type: "string", value: "sensor-align" },
    extCapturetype: { type: "string", value: "ext-capturetype" },
    uvSecurity: { type: "boolean", value: "uvsecurity" },
    paperDetectFreq: { type: "number", value: "paperdetectfreq" },
    scanCount: { type: "number", value: "scancount" },
    calibrate: { type: "boolean", value: "calibrate" },
    imageFmt: { type: "string", value: "imagefmt" },
    imageCustomInfo: { type: "boolean", value: "imagecustominfo" },
    imgSoftware: { type: "string", value: "imgsoftware" },
    imgArtist: { type: "string", value: "imgartist" },
    filenameFormat: { type: "string", value: "filename-format" },
    filenameBeginindex: { type: "number", value: "filename-beginindex" },
    photo: { type: "boolean", value: "photo" },
    watermark: { type: "string", value: "watermark" },
    savePath: { type: "string", value: "savepath" }, // 2.0 default ini
    removePunchHole: { type: "boolean", value: "remove-punchhole" },
    removeBlackEdges: { type: "boolean", value: "remove-blackedges" },
    removeBlankPage: { type: "boolean", value: "remove-blankpage" },
    removeBlankpageSensitivity: {
      type: "number",
      value: "remove-blankpage-sensitivity",
    },
    removeBackground: { type: "boolean", value: "remove-background" },
    removeBackgroundFillcolor: {
      type: "number",
      value: "remove-background-fillcolor",
    },
    removeBackgroundWeight: {
      type: "number",
      value: "remove-background-weight",
    },
    removeBackgroundRadius: {
      type: "number",
      value: "remove-background-radius",
    },
    resolution: { type: "number", value: "resolution" },
    resizeDpi: { type: "number", value: "resizedpi" },
    brightness: { type: "number", value: "brightness" },
    contrast: { type: "number", value: "contrast" },
    quality: { type: "number", value: "quality" },
    bgSensitivity: { type: "number", value: "bgsensitivity" },
    gamma: { type: "number", value: "gamma" },
    saturationShift: { type: "number", value: "saturationshift" },
    gridArea: { type: "string", value: "gridarea" },
    gridSensitivity: { type: "number", value: "grid-sensitivity" },
    colorDropout: { type: "string", value: "colordropout" },
    fillEdgeColor: { type: "string", value: "filledgecolor" },
    applyColorProfile: { type: "string", value: "applycolorprofile" },
    colorProfileGamma: { type: "number", value: "colorprofilegamma" },
    barcodeAreas: { type: "string", value: "barcodeareas" },
    barcodeType: { type: "boolean", value: "barcode-type" },
    swdeskew: { type: "boolean", value: "swdeskew" },
    swcrop: { type: "boolean", value: "swcrop" },
    innerCutted: { type: "number", value: "innercutted" },
    crop: { type: "string", value: "crop" },
    multicrop: { type: "number", value: "multicrop" },
    hasBlackImage: { type: "boolean", value: "hasblackimage" },
    uiRectNum: { type: "number", value: "uirectnum" },
    multiCropRect: { type: "string", value: "multicroprect" },
    sharpen: { type: "boolean", value: "sharpen" },
    denoise: { type: "boolean", value: "denoise" },
    density: { type: "boolean", value: "density" },
    charEnhance: { type: "boolean", value: "charenhance" },
    redTypeEnhance: { type: "boolean", value: "redtypeenhance" },
    backwardEject: { type: "boolean", value: "backward-eject" },
    thumbNail: { type: "boolean", value: "thumbnail" },
    multiFeed: { type: "boolean", value: "multifeed" },
    bottomUp: { type: "boolean", value: "bottomup" },
    rawData: { type: "boolean", value: "rawdata" },
    borderFill: { type: "boolean", value: "borderfill" },
    jpegXfer: { type: "boolean", value: "jpegxfer" },
    base64enc: { type: "boolean", value: "base64enc" },
    orig: { type: "boolean", value: "orig" },
    log: { type: "boolean", value: "log" },
  };

  // Get all properties except for interfaceTable.
  static getOptions() {
    const data = {
      devices: this.devices,
      scannerOption: this.scannerOption,
      scanOption: this.scanOption,
      imageOption: this.imageOption,
      ocrOption: this.ocrOption,
      exportOption: this.exportOption,
    };

    // deep copy
    return JSON.parse(JSON.stringify(data));
  }

  // Conversion of property names between the "lib" and "sdk" sides. Property names in the "lib" side are consistently in camel case, while in the "sdk" side, they vary between all lowercase, camel case, and dash-separated names.
  static paramFormat(paramObj) {
    const self = this;
    let fixParamObj = { ...paramObj };
    let newParamObj = {};

    // In webFXScan 2.0, some parameters are not supported, which may result in the server responding abnormally. Preprocessing will be done here to handle this.
    if (paramObj.recognizeType === "none") {
      const { recognizeType, recognizeLang, fullTextType, ...other } = paramObj;
      fixParamObj = { ...other };
    }

    // In webFXScan 2.0, when scanning, this parameter must be present to return base64. It will be used by default in typical scenarios.
    fixParamObj = { ...fixParamObj, base64enc: true };

    Object.keys(fixParamObj).map(function (key) {
      if (key in self.interfaceTable) {
        const fixedKey = self.interfaceTable[key].value;
        const targetType = self.interfaceTable[key].type;
        const targetValue = fixParamObj[key];
        const fixedValue =
          targetType === "number"
            ? Number(targetValue)
            : targetType === "boolean"
            ? Boolean(targetValue)
            : targetType === "string"
            ? String(targetValue)
            : targetValue;
        newParamObj[fixedKey] = fixedValue;
      } else {
        // Unsupported inputs will be skipped.
      }
    });
    return newParamObj;
  }
}

/* Utility */
class WebFxScanUtility {
  static errorCode = {
    apiNotExist: "api not exist",
    connectFail: "connect fail",
    apiIsBusy: "api is busy",
    connectExist: "connect is exist",
    connectAbort: "connect abort",
    connectClose: "connect close",
    connectError: "connect error",
    closeFail: "close fail",
    timeout: "timeout",
    unknown: "unexpect error",
    0: "LIBWFX_ERRCODE_SUCCESS",
    1: "LIBWFX_ERRCODE_FAIL",
    2: "LIBWFX_ERRCODE_NO_INIT",
    3: "LIBWFX_ERRCODE_NOT_YET_OPEN_DEVICE",
    4: "LIBWFX_ERRCODE_DEVICE_ALREADY_OPEN",
    5: "LIBWFX_ERRCODE_INVALID_SOURCE",
    6: "LIBWFX_ERRCODE_NO_ENABLE_THRESHOLD",
    7: "LIBWFX_ERRCODE_NO_SUPPORT_THRESHOLD",
    8: "LIBWFX_ERRCODE_NOT_YET_SET_SCAN_PROPERTY",
    9: "LIBWFX_ERRCODE_NO_SET_RECOGNIZE_TOOL",
    10: "LIBWFX_ERRCODE_OCR_NOT_SUPPORT_BOTTOMUP",
    11: "LIBWFX_ERRCODE_READ_IMAGE_FAILED",
    12: "LIBWFX_ERRCODE_ONLY_SUPPORT_COLOR_MODE",
    13: "LIBWFX_ERRCODE_ICM_PROFILE_NOT_EXIST",
    14: "LIBWFX_ERRCODE_NO_SUPPORT_EJECT",
    15: "LIBWFX_ERRCODE_NO_SUPPORT_JPEGXFER",
    16: "LIBWFX_ERRCODE_PAPER_NOT_READY",
    17: "LIBWFX_ERRCODE_INVALID_SERIALNUM",
    18: "LIBWFX_ERRCODE_DISCONNECT",
    19: "LIBWFX_ERRCODE_FORMAT_NOT_SUPPORT",
    20: "LIBWFX_ERRCODE_NO_CALIBRATION_DATA",
    21: "LIBWFX_ERRCODE_OCR_TOOL_NOT_SUPPORT",
    22: "LIBWFX_ERRCODE_RECOGNIZE_TYPE_NOT_SUPPORT",
    23: "LIBWFX_ERRCODE_INVALID_CERTIFICATE",
    24: "LIBWFX_ERRCODE_AP_ALREADY_EXISIT",
    25: "LIBWFX_ERRCODE_OPENs_REGISTRY_KEY_FAILED",
    26: "LIBWFX_ERRCODE_LOAD_MRTD_DLL_FAIL",
    27: "LIBWFX_ERRCODE_COVER_OPENED",
    28: "LIBWFX_ERRCODE_CERTIFICATE_EXPIRED",
    29: "LIBWFX_ERRCODE_ALREADY_INIT",
    30: "LIBWFX_ERRCODE_NO_SUPPORT_DUPLEX",
    1001: "LIBWFX_ERRCODE_NO_AVI_OCR",
    1002: "LIBWFX_ERRCODE_NO_DOC_OCR",
    1003: "LIBWFX_ERRCODE_NO_OCR",
    1004: "LIBWFX_ERRCODE_NO_DEVICES",
    1005: "LIBWFX_ERRCODE_NO_DEVICE_NAME",
    1006: "LIBWFX_ERRCODE_NO_SOURCE",
    1007: "LIBWFX_ERRCODE_FILE_NOT_EXIST",
    1008: "LIBWFX_ERRCODE_PATH_TOO_LONG",
    1009: "LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH",
    1010: "LIBWFX_ERRCODE_SCANNING",
    1011: "LIBWFX_ERRCODE_FILE_OCCUPIED",
    1012: "LIBWFX_ERRCODE_SAVEPATH_ERROR",
    1013: "LIBWFX_ERRCODE_TIMEOUT",
  };
  static eventCode = {
    0: "LIBWFX_EVENT_PAPER_DETECTED",
    1: "LIBWFX_EVENT_NO_PAPER",
    2: "LIBWFX_EVENT_PAPER_JAM",
    3: "LIBWFX_EVENT_MULTIFEED",
    4: "LIBWFX_EVENT_NO_CALIBRATION_DATA",
    5: "LIBWFX_EVENT_WARMUP_COUNTDOWN",
    6: "LIBWFX_EVENT_SCAN_PROGRESS",
    7: "LIBWFX_EVENT_BUTTON_DETECTED",
    8: "LIBWFX_EVENT_SCANNING",
    9: "LIBWFX_EVENT_PAPER_FEEDING_ERROR",
    10: "LIBWFX_EVENT_COVER_OPEN",
    11: "LIBWFX_EVENT_LEFT_SENSOR_DETECTED",
    12: "LIBWFX_EVENT_RIGHT_SENSOR_DETECTED",
    13: "LIBWFX_EVENT_ALL_SENSOR_DETECTED",
    14: "LIBWFX_EVENT_UVSECURITY_DETECTED",
    15: "LIBWFX_EVENT_PLUG_UNPLUG",
    16: "LIBWFX_EVENT_OVER_TIME_SCAN",
    17: "LIBWFX_EVENT_CANCEL_SCAN",
    18: "LIBWFX_EVENT_CAMERA_RGB_DISLOCATION",
    19: "LIBWFX_EVENT_CAMERA_TIMEOUT",
  };
  static unknownErrorTypeMsg = "undefined error";

  // API 拋出錯誤
  static throwError(props) {
    const { errCode, apiName } = props;
    if (errCode in this.errorCode) {
      throw `${apiName}: ${this.errorCode[errCode]}`;
    } else {
      throw `${apiName}: ${this.unknownErrorTypeMsg}`;
    }
  }
  // API failure return format
  static failureResponse(props) {
    const self = this;
    const { errCode = 0, message = "", apiItem = null } = props;
    // if (apiItem !== null) apiItem.isActive = false;
    return {
      result: false,
      message: message,
      error:
        errCode in self.errorCode
          ? self.errorCode[errCode]
          : self.unknownErrorTypeMsg,
    };
  }
  // API successful return format
  static successResponse(props) {
    const { message = "", data = {}, apiItem = null } = props;
    // if (apiItem !== null) apiItem.isActive = false;
    return { result: true, data, message };
  }

  // The return format of WebFXScan includes nested JSON strings. Here, traversal through layers for conversion is required.
  static sdkV2MessageParser(jsonStr) {
    const nonEscapeJsonStr = jsonStr.replace(/\\r\\n/g, "");

    try {
      let obj = JSON.parse(nonEscapeJsonStr);
      return traverseObject(obj);
    } catch (error) {
      console.warn("sdkV2MessageParser error:", error);
    }

    function traverseObject(obj) {
      let result = obj;
      for (let key in result) {
        if (typeof result[key] === "string") {
          try {
            const tempVar = JSON.parse(result[key]);
            result[key] = tempVar;
            if (typeof tempVar === "object") traverseObject(tempVar);
          } catch (error) {
            // skip
          }
        } else if (typeof result[key] === "object") {
          traverseObject(result[key]);
        } else {
          // ingore other type
        }
      }
      return result;
    }
  }

  // Path parser for webFXScan 2.0
  static pathParser(path = "") {
    const fullName = path.split("\\").pop();
    const name = fullName.split(".").slice(0, -1).join(".");
    const ext = fullName.split(".").pop();
    return { fullName, fileName: name, ext };
  }

  // supported base64 type
  static base64Parser(base64, ext) {
    const MIMElist = {
      bmp: "data:image/bmp;base64,",
      jpg: "data:image/jpeg;base64,",
      png: "data:image/png;base64,",
      tif: "data:image/tiff;base64,",
      pdf: "data:application/pdf;base64,",
      pnm: "data:image/x-portable-anymap;base64,", // Note: The browser may have limited support.
      spdf: "data:application/vnd.adobe.structureddoc;base64,", // Note: The browser may have limited support.
      txt: "data:text/plain;base64,",
      rtf: "data:application/rtf;base64,",
      xls: "data:application/vnd.ms-excel;base64,",
    };

    if (ext.toLowerCase() in MIMElist) return `${MIMElist[ext]}${base64}`;
    else return base64;
  }
}

/* Ws server side interface encapsulation */
class WebFxScanServer {
  constructor(props) {
    this.cache = JSON.parse(JSON.stringify(this.initCache));
    const { mode } = props;
    this.state.mode = mode;
  }

  version = "1.1.1";
  state = {
    socket: null,
    ip: "",
    port: "",
    timeout: 60000, // ms
    mode: "prod", // "prod" | "dev", dev provide more console.log
  };
  // webFXScan api list, and the same API cannot be enabled at the same time.
  // command: server side method
  // receive: server side response mapping (multi maybe)
  // method: lib business logic for [command]
  // callback: lib business logic for [receive]
  apiList = {
    close: {
      command: { type: "", func: "" },
      receive: [{ type: "", func: "" }],
      method: () => {},
      callback: () => {},
      promise: null,
      isActive: false,
      timeoutId: null,
      timeout: null,
    },
    connect: {
      command: { type: "", func: "" },
      receive: [{ type: "", func: "" }],
      method: () => {},
      callback: () => {},
      promise: null,
      isActive: false,
      timeoutId: null,
      timeout: null,
    },
    init: {
      command: { type: "call", func: "LibWFX_Init" },
      receive: [{ type: "return", func: "LibWFX_Init" }],
      method: this.init,
      callback: this.callbackInit,
      promise: null,
      isActive: false,
      timeoutId: null,
      timeout: null,
    },
    getDeviceList: {
      command: { type: "call", func: "LibWFX_GetDeviesList" },
      receive: [{ type: "return", func: "LibWFX_GetDeviesList" }],
      method: this.getDeviceList,
      callback: this.callbackGetDeviceList,
      promise: null,
      isActive: false,
      timeoutId: null,
      timeout: null,
    },
    getDeviceCap: {
      command: { type: "call", func: "LibWFX_GetDeviceCapability" },
      receive: [{ type: "return", func: "LibWFX_GetDeviceCapability" }],
      method: this.getDeviceCap,
      callback: this.callbackGetDeviceCap,
      promise: null,
      isActive: false,
      timeoutId: null,
      timeout: null,
    },
    getFileList: {
      command: { type: "call", func: "LibWFX_GetFileList" },
      receive: [{ type: "return", func: "LibWFX_GetFileList" }],
      method: this.getFileList,
      callback: this.callbackGetFileList,
      promise: null,
      isActive: false,
      timeoutId: null,
      timeout: null,
    },
    getFile: {
      command: { type: "call", func: "LibWFX_GetFileData" },
      receive: [{ type: "return", func: "LibWFX_GetFileData" }],
      method: this.getFile,
      callback: this.callbackGetFile,
      promise: null,
      isActive: false,
      timeoutId: null,
      timeout: null,
    },
    setProperty: {
      command: { type: "call", func: "LibWFX_SetProperty" },
      receive: [{ type: "return", func: "LibWFX_SetProperty" }],
      method: this.setProperty,
      callback: this.callbackSetProperty,
      promise: null,
      isActive: false,
      timeoutId: null,
      timeout: null,
    },
    scan: {
      command: { type: "call", func: "LibWFX_AsynchronizeScan" },
      receive: [
        { type: "return", func: "LibWFX_AsynchronizeScan" },
        { type: "callback", func: "LIBWFX_NOTIFY_IMAGE_DONE" },
        { type: "callback", func: "LIBWFX_NOTIFY_END" },
      ],
      method: this.scan,
      callback: this.callbackScan,
      promise: null,
      isActive: false,
      timeoutId: null,
      timeout: null,
    },
    convert: {
      command: { type: "call", func: "LibWFX_ExportReadImageByFileName" },
      receive: [{ type: "return", func: "LibWFX_ExportReadImageByFileName" }],
      method: this.convert,
      callback: this.callbackConvert,
      promise: null,
      isActive: false,
      timeoutId: null,
      timeout: null,
    },
    mergePdf: {
      command: { type: "call", func: "LibWFX_MergePdfByFileName" },
      receive: [{ type: "return", func: "LibWFX_MergePdfByFileName" }],
      method: this.mergePdf,
      callback: this.callbackMergePdf,
      promise: null,
      isActive: false,
      timeoutId: null,
      timeout: null,
    },
    imageProcess: {
      command: { type: "call", func: "LibWFX_ReadImageByFileName" },
      receive: [{ type: "return", func: "LibWFX_ReadImageByFileName" }],
      method: this.imageProcess,
      callback: this.callbackImageProcess,
      promise: null,
      isActive: false,
      timeoutId: null,
      timeout: null,
    },
    deleteAll: {
      command: { type: "call", func: "LibWFX_RecycleSaveFolder" },
      receive: [{ type: "return", func: "LibWFX_RecycleSaveFolder" }],
      method: this.deleteAll,
      callback: this.callbackDeleteAll,
      promise: null,
      isActive: false,
      timeoutId: null,
      timeout: null,
    },
    deleteFile: {
      command: { type: "call", func: "LibWFX_DeleteFile" },
      receive: [{ type: "return", func: "LibWFX_DeleteFile" }],
      method: this.deleteFile,
      callback: this.callbackDeleteFile,
      promise: null,
      isActive: false,
      timeoutId: null,
      timeout: null,
    },
  };

  initCache = {
    scannerProperty: {},
    tempScanData: [], // cache scan() response data
    autoScanCallback: null, // cache callback with autoScan()
    scanCallback: null, // cache callback with scan()
    scanIndex: 0, // progress
  };
  cache = {};
  isDisconnecting = false;

  // bridge for apiList
  notify(apiName, data = null, timeout = null) {
    const self = this;
    if (!(apiName in self.apiList)) {
      WebFxScanUtility.throwError({ errCode: "apiNotExist", apiName });
    }
    if (self.apiList[apiName].isActive) {
      WebFxScanUtility.throwError({ errCode: "apiIsBusy", apiName });
    }

    return new Promise((resolve, reject) => {
      // update apiList promise status
      delete self.apiList[apiName].promise;
      self.apiList[apiName].promise = { resolve, reject };
      self.apiList[apiName].isActive = true;
      if (typeof timeout === "number") self.apiList[apiName].timeout = timeout;

      // send ws api
      const { type, func } = self.apiList[apiName].command;
      const message = {
        type,
        func,
        ...(data === null ? {} : { data: data }),
      };
      self.devLog(
        "[ScanLib] websocket socket.send data:",
        JSON.stringify(message)
      );
      self.state.socket.send(JSON.stringify(message));

      // reject if timeout
      self.startTimeout(apiName);
    });
  }

  // reset class' state, cache, apiList
  initConnectState() {
    const self = this;
    self.state.socket = null;
    self.cache = JSON.parse(JSON.stringify(self.initCache));

    for (const [key, value] of Object.entries(self.apiList)) {
      self.apiList[key].isActive = false;
      // do not clear promise
      // self.apiList[key].promise = null;
      if (self.apiList[key].timeoutId) {
        clearTimeout(self.apiList[key].timeoutId);
      }
      self.apiList[key].timeoutId = null;
      self.apiList[key].timeout = null;
    }
  }

  startTimeout(apiName) {
    const self = this;
    const timeout =
      typeof self.apiList[apiName].timeout === "number"
        ? self.apiList[apiName].timeout
        : self.state.timeout;

    self.apiList[apiName].timeoutId = setTimeout(() => {
      self.devLog(
        `[ScanLib] timeout trigger, id=self.apiList[apiName].timeoutId`,
        `api name=${apiName}`
      );

      self.promiseReject(
        self.apiList[apiName],
        WebFxScanUtility.failureResponse({
          errCode: "timeout",
          message: `error: ${apiName} timeout > ${timeout / 1000}s`,
        })
      );
    }, timeout);
    self.devLog(
      `[ScanLib] new startTimeout id=${self.apiList[apiName].timeoutId}`,
      `api name=${apiName}`
    );
  }

  resetTimeout(apiName) {
    const self = this;
    if (self.apiList[apiName].timeoutId) {
      clearTimeout(self.apiList[apiName].timeoutId);
    }
    self.startTimeout(apiName);
  }

  devLog(...args) {
    const self = this;
    if (self.state.mode === "dev") {
      console.log(...args);
    }
  }

  promiseReject(apiItem, message, beforeCallback = () => {}) {
    const self = this;
    if (apiItem.isActive) {
      self.devLog("[ScanLib] reject message:", message);
      beforeCallback();
      clearTimeout(apiItem.timeoutId);
      apiItem.isActive = false;
      apiItem.timeout = null;
      apiItem.promise.reject(message);
    }
  }

  promiseResolve(apiItem, message, beforeCallback = () => {}) {
    const self = this;
    if (apiItem.isActive) {
      self.devLog("[ScanLib] resolve message:", message);
      beforeCallback();
      clearTimeout(apiItem.timeoutId);
      apiItem.isActive = false;
      apiItem.timeout = null;
      apiItem.promise.resolve(message);
    }
  }

  // API: connect webFXScan server
  async connect(props) {
    const self = this;
    const {
      ip = "127.0.0.1",
      port = "17778",
      errorCallback,
      closeCallback,
    } = props;
    self.initConnectState();
    return new Promise((resolve, reject) => {
      // Temporary promise, used to interrupt when a connection is close() during establishment.
      self.apiList.connect.promise = { resolve, reject };
      self.apiList.connect.isActive = true;

      // reject if connection is exist
      if (self.state.socket !== null) {
        self.promiseReject(
          self.apiList.connect,
          WebFxScanUtility.failureResponse({
            errCode: "connectExist",
            message: "",
          })
        );
        return;
      }

      self.state.ip = ip;
      self.state.port = port;
      let socket;

      try {
        socket = new WebSocket("ws://" + ip + ":" + port + "/webscan2");
      } catch (e) {
        self.devLog("[ScanLib] connect() new WebSocket error:", e);
        // self.state.socket = null;
        self.promiseReject(
          self.apiList.connect,
          WebFxScanUtility.failureResponse({
            errCode: "connectFail",
            message: "",
          })
        );
        return;
      }

      socket.onopen = () => {
        self.devLog("[ScanLib] open connection success");
        self.state.socket = socket;
        self.promiseResolve(
          self.apiList.connect,
          WebFxScanUtility.successResponse({
            message: "OK",
          })
        );
      };
      socket.onclose = function (event) {
        // When executing close(), it needs to wait until onClose is triggered to consider it as the promise being fulfilled.
        if (self.isDisconnecting) {
          self.isDisconnecting = false;
          self.promiseResolve(
            self.apiList.close,
            WebFxScanUtility.successResponse({
              message: "OK",
            })
          );
        }

        // Standard handling, occurring when the server disconnects or due to other reasons.
        self.devLog("[ScanLib] onclose event:", event);
        self.initConnectState();
        closeCallback(event);
        self.promiseReject(
          self.apiList.connect,
          WebFxScanUtility.failureResponse({
            errCode: "connectClose",
            message: "websocket on close",
          })
        );
      };
      socket.onerror = function (event) {
        self.devLog("[ScanLib] onerror event:", event);
        errorCallback(event);
        self.promiseReject(
          self.apiList.connect,
          WebFxScanUtility.failureResponse({
            errCode: "connectError",
            message: event.message,
          })
        );
      };
      socket.onmessage = function (event) {
        try {
          self.devLog("[ScanLib] websocket onmessage event.data:", event.data);
          const fixData = WebFxScanUtility.sdkV2MessageParser(event.data);
          const { type, func, data } = fixData;

          Object.keys(self.apiList).map((apiName) => {
            self.apiList[apiName].receive.map((receiveItem) => {
              const { type: receiveType, func: receiveFunc } = receiveItem;
              if (receiveType === type && receiveFunc === func) {
                self.apiList[apiName].callback(self, fixData);
              }
            });
          });
        } catch (error) {
          self.devLog("[ScanLib] onmessage catch error:", error);
        }
      };
    });
  }

  // API: colse webFXScan connection
  async close(props = {}) {
    const self = this;
    return new Promise((resolve, reject) => {
      self.apiList.close.promise = { resolve, reject };
      self.apiList.close.isActive = true;
      /* if connect() is running, it will abort it */
      if (self.apiList.connect.isActive) {
        self.promiseReject(
          self.apiList.connect,
          WebFxScanUtility.failureResponse({
            errCode: "connectAbort",
            message: "websocket connect abort by close()",
          })
        );
      }
      /* reset status */
      try {
        // if connection is exist
        if (self.state.socket) {
          self.state.socket.close();
          // wait for websocket onClose
          self.isDisconnecting = true;
        } else {
          self.promiseResolve(
            self.apiList.close,
            WebFxScanUtility.successResponse({
              message: "OK",
            })
          );
        }
      } catch (e) {
        WebFxScanUtility.failureResponse({
          errCode: "closeFail",
          message: "websocket socket.close faile by close()",
        });
      }
    });
  }

  // API: Initialization, must be called at least once before executing other APIs.
  async init() {
    const self = this;
    return self.notify("init");
  }
  callbackInit(self, fixData) {
    const apiItem = this;
    try {
      const { type, func, data } = fixData;
      const { err_code = 0, message = "" } = data;

      if (err_code === 0 || [1001, 1002, 1003].includes(err_code)) {
        self.promiseResolve(
          apiItem,
          WebFxScanUtility.successResponse({ message: "OK", apiItem })
        );
      } else {
        self.promiseReject(
          apiItem,
          WebFxScanUtility.failureResponse({
            errCode: err_code,
            message,
            apiItem,
          })
        );
      }
    } catch (error) {
      self.promiseReject(
        apiItem,
        WebFxScanUtility.failureResponse({
          errCode: "unknown",
          message: error,
          apiItem,
        })
      );
    }
  }

  // API: get device list and options
  async getDeviceList() {
    const self = this;
    return self.notify("getDeviceList");
  }
  callbackGetDeviceList(self, fixData) {
    const apiItem = this;
    try {
      const { type, func, data } = fixData;
      const { err_code = 0, message = "" } = data;
      const { szDevicesList, szSerialList } = message;

      if (err_code !== 0) {
        self.promiseReject(
          apiItem,
          WebFxScanUtility.failureResponse({
            errCode: err_code,
            message,
            apiItem,
          })
        );
      }

      const {
        devices,
        scannerOption,
        scanOption,
        imageOption,
        ocrOption,
        exportOption,
      } = WebFxScanScanner.getOptions();
      const options = [];
      const groupOptions = [];
      szDevicesList.map((device) => {
        const type = devices[device];
        const confEnum = scannerOption[type];
        let v2ScannerOption = false;
        (async () => {
          try {
            const deviceCap = await self.getDeviceCap({
              deviceName: device,
            });
            const { data } = deviceCap;
            v2ScannerOption = data;
          } catch (error) {
            self.devLog("[ScanLib] v2ScannerOption error:", v2ScannerOption);
          }

          groupOptions.push({
            scanId: device,
            scanOption: {
              deviceName: { value: szDevicesList, type: "list" },
              // ...confEnum,
              ...(v2ScannerOption ? v2ScannerOption : confEnum),
              ...scanOption,
            },
            imageOption: {
              ...imageOption,
            },
            ocrOption: {
              ...ocrOption,
            },
            exportOption: {
              ...exportOption,
            },
          });
          options.push({
            deviceName: device,
            ...scanOption,
            ...imageOption,
            ...ocrOption,
            ...exportOption,
            ...(v2ScannerOption ? v2ScannerOption : confEnum),
          });

          self.promiseResolve(
            apiItem,
            WebFxScanUtility.successResponse({
              message: "OK",
              data: { options, groupOptions },
              apiItem,
            })
          );
        })();
      });
    } catch (error) {
      self.promiseReject(
        apiItem,
        WebFxScanUtility.failureResponse({
          errCode: "unknown",
          message: error,
          apiItem,
        })
      );
    }
  }

  // Get available scanner property form webFXScan server
  async getDeviceCap(props) {
    const { deviceName } = props;
    const self = this;

    return self.notify("getDeviceCap", { "device-name": deviceName });
  }

  callbackGetDeviceCap(self, fixData) {
    const apiItem = this;
    try {
      const { type, func, data } = fixData;
      const { err_code = 0, message } = data;

      if (err_code === 0) {
        const {
          devicename,
          source,
          duplex,
          jpegtransfer,
          dpi,
          max_papersize_x,
          max_papersize_y,
          longpaper,
        } = message;

        const sourceMap = [
          {
            source: 0, // unknown
            duplex: true,
            data: {
              value: [],
              type: "list",
            },
          },
          {
            source: 0, // unknown
            duplex: false,
            data: {
              value: [],
              type: "list",
            },
          },
          {
            source: 1, // ADF
            duplex: true,
            data: {
              value: ["ADF-Front", "ADF-Back", "ADF-Duplex"],
              type: "list",
            },
          },
          {
            source: 1, // ADF
            duplex: false,
            data: {
              value: ["ADF-Front"],
              type: "list",
            },
          },
          {
            source: 2, // Sheetfed
            duplex: true,
            data: {
              value: ["Sheetfed-Front", "Sheetfed-Back", "Sheetfed-Duplex"],
              type: "list",
            },
          },
          {
            source: 2, // Sheetfed
            duplex: false,
            data: {
              value: ["Sheetfed-Front"],
              type: "list",
            },
          },
          {
            source: 3, // ADF + Sheetfed
            duplex: true,
            data: {
              value: [
                "Auto",
                "ADF-Front",
                "ADF-Back",
                "ADF-Duplex",
                "Sheetfed-Front",
                "Sheetfed-Back",
                "Sheetfed-Duplex",
              ],
              type: "list",
            },
          },
          {
            source: 3, // ADF + Sheetfed
            duplex: false,
            data: {
              value: ["Auto", "ADF-Front", "Sheetfed-Front"],
              type: "list",
            },
          },
          {
            source: 4, // Flatbed
            duplex: true,
            data: {
              value: ["Flatbed"],
              type: "list",
            },
          },
          {
            source: 4, // Flatbed
            duplex: false,
            data: {
              value: ["Flatbed"],
              type: "list",
            },
          },
          {
            source: 5, // ADF + Flatbed
            duplex: true,
            data: {
              value: ["Auto", "ADF-Front", "ADF-Back", "ADF-Duplex", "Flatbed"],
              type: "list",
            },
          },
          {
            source: 5, // ADF + Flatbed
            duplex: false,
            data: {
              value: ["Auto", "ADF-Front", "Flatbed"],
              type: "list",
            },
          },
          {
            source: 6, // CAMERA
            duplex: true,
            data: {
              value: ["Camera"],
              type: "list",
            },
          },
          {
            source: 6, // CAMERA
            duplex: false,
            data: {
              value: ["Camera"],
              type: "list",
            },
          },
        ];

        const paperSizeMap = [
          {
            maxPapersizeX: 11.69,
            maxPapersizeY: 16.54,
            value: "A3",
          },
          {
            maxPapersizeX: 8.27,
            maxPapersizeY: 11.69,
            value: "A4",
          },

          {
            maxPapersizeX: 5.8,
            maxPapersizeY: 8.3,
            value: "A5",
          },
          {
            maxPapersizeX: 4.1,
            maxPapersizeY: 5.8,
            value: "A6",
          },
          {
            maxPapersizeX: 8.5,
            maxPapersizeY: 11,
            value: "Letter",
          },
          {
            maxPapersizeX: 6.9,
            maxPapersizeY: 9.8,
            value: "B5",
          },
          {
            maxPapersizeX: 4.9,
            maxPapersizeY: 6.9,
            value: "B6",
          },
        ];

        // parser source
        const filterSource = sourceMap.filter((item) => {
          const { source: checkSource, duplex: checkDuplex } = item;
          if (source === checkSource && duplex === checkDuplex) {
            return true;
          } else {
            return false;
          }
        });
        const availableSource = filterSource?.[0]
          ? filterSource[0].data
          : {
              value: [],
              type: "list",
            };

        // parser paperSize
        const filterPaperSize = paperSizeMap
          .filter((item) => {
            const { maxPapersizeX, maxPapersizeY } = item;
            if (
              max_papersize_x >= maxPapersizeX &&
              max_papersize_y >= maxPapersizeY
            ) {
              return true;
            } else {
              return false;
            }
          })
          .map((item) => {
            return item.value;
          });

        const availablePaperSize = longpaper
          ? {
              value: filterPaperSize.push("LongPaper").unshift("Auto"),
              type: "list",
            }
          : {
              value: filterPaperSize,
              type: "list",
            };

        // parser mode
        const availableMode =
          source === 6
            ? {
                value: ["color"],
                type: "list",
              }
            : {
                value: ["lineart", "gray", "color"],
                type: "list",
              };

        // parser resolution
        const availableResolution = {
          value: [0, dpi],
          type: "range",
        };

        const result = {
          source: availableSource,
          paperSize: availablePaperSize,
          resolution: availableResolution,
          mode: availableMode,
        };

        self.promiseResolve(
          apiItem,
          WebFxScanUtility.successResponse({
            message: "OK",
            data: result,
            apiItem,
          })
        );
      } else {
        self.promiseReject(
          apiItem,
          WebFxScanUtility.failureResponse({
            errCode: err_code,
            message,
            apiItem,
          })
        );
      }
    } catch (error) {
      self.promiseReject(
        apiItem,
        WebFxScanUtility.failureResponse({
          errCode: "unknown",
          message: error,
          apiItem,
        })
      );
    }
  }

  // API: get file list
  async getFileList() {
    const self = this;
    return self.notify("getFileList");
  }
  callbackGetFileList(self, fixData) {
    const apiItem = this;
    try {
      const { type, func, data } = fixData;
      const { err_code = 0, message } = data;

      if (err_code === 0) {
        (async () => {
          const result = await processArray(message);
          self.promiseResolve(
            apiItem,
            WebFxScanUtility.successResponse({
              message: "OK",
              data: result,
              apiItem,
            })
          );
        })();
      } else {
        self.promiseReject(
          apiItem,
          WebFxScanUtility.failureResponse({
            errCode: err_code,
            message,
            apiItem,
          })
        );
      }

      async function processArray(array) {
        const result = [];
        for (const path of array) {
          const { fullName, fileName, ext } = WebFxScanUtility.pathParser(path);
          const allowExtension = ["jpg", "png", "bmp"]; // filte other type
          if (!allowExtension.includes(ext)) continue;
          // get image
          const { data: imageData } = await self.getFile({
            filename: fullName,
          });
          const { base64 = "" } = imageData;
          // get ocr text
          const { data: textData } = await self.getFile({
            filename: `${fileName}_OCR.txt`,
          });
          const { recognizedata = "" } = textData;

          if (base64.length) {
            result.push({
              fileName: fullName,
              base64,
              ocrText: recognizedata,
            });
          }

          self.resetTimeout("getFileList");
        }
        return result;
      }
    } catch (error) {
      self.promiseReject(
        apiItem,
        WebFxScanUtility.failureResponse({
          errCode: "unknown",
          message: error,
          apiItem,
        })
      );
    }
  }

  // API: get sungle file
  async getFile(props) {
    const self = this;
    const { filename } = props;
    const data = { filename };
    return self.notify("getFile", data);
  }
  callbackGetFile(self, fixData) {
    const apiItem = this;
    try {
      const { type, func, data } = fixData;
      const { err_code = 0, message } = data;
      const { name, base64, recognizedata } = message;
      const { ext } = WebFxScanUtility.pathParser(name);

      const result = {
        name,
        base64: WebFxScanUtility.base64Parser(base64, ext),
        recognizedata,
      };
      self.promiseResolve(
        apiItem,
        WebFxScanUtility.successResponse({
          message: "OK",
          data: result,
          apiItem,
        })
      );
    } catch (error) {
      self.promiseReject(
        apiItem,
        WebFxScanUtility.failureResponse({
          errCode: "unknown",
          message: error,
          apiItem,
        })
      );
    }
  }

  // API: set scanner properties
  async setProperty(props) {
    const { scanParam, autoScan, callback } = props;
    const self = this;
    let fixScanParam = {};

    if (autoScan) {
      fixScanParam = {
        ...self.cache.scannerProperty,
        autoscan: true,
      };
      self.cache.autoScanCallback = callback;
    } else {
      // parser
      fixScanParam = WebFxScanScanner.paramFormat(scanParam);
    }

    // cahce for some API
    self.cache.scannerProperty = fixScanParam;
    return self.notify("setProperty", fixScanParam);
  }
  callbackSetProperty(self, fixData) {
    const apiItem = this;
    try {
      const { type, func, data } = fixData;
      const { err_code = 0, message } = data;

      if (err_code === 0) {
        self.promiseResolve(
          apiItem,
          WebFxScanUtility.successResponse({
            message: "OK",
            apiItem,
          })
        );
      } else {
        self.promiseReject(
          apiItem,
          WebFxScanUtility.failureResponse({
            errCode: err_code,
            message,
            apiItem,
          })
        );
      }
    } catch (error) {
      self.promiseReject(
        apiItem,
        WebFxScanUtility.failureResponse({
          errCode: "unknown",
          message: error,
          apiItem,
        })
      );
    }
  }

  // API: scan
  async scan(props) {
    const { callback = null, timeout } = props;
    const self = this;

    if (typeof callback === "function") {
      self.cache.scanCallback = callback;
    }

    return self.notify("scan", null, timeout);
  }
  callbackScan(self, fixData) {
    const apiItem = this;
    try {
      const { type, func, data } = fixData;
      const { err_code = 0, message } = data;

      if (err_code !== 0) {
        self.cache.scanIndex = 0;
        self.promiseReject(
          apiItem,
          WebFxScanUtility.failureResponse({
            errCode: err_code,
            message,
            apiItem,
          })
        );
      }

      if (func === "LIBWFX_NOTIFY_END") {
        self.cache.scanIndex = 0;
        self.promiseResolve(
          apiItem,
          WebFxScanUtility.successResponse({
            message: "OK",
            data: self.cache.tempScanData,
            apiItem,
          })
        );
      } else if (func === "LibWFX_AsynchronizeScan") {
        self.cache.tempScanData = [];
        return;
      } else if (func === "LIBWFX_NOTIFY_IMAGE_DONE") {
        self.resetTimeout("scan");
        const { name, base64, recognizedata, md5 = "" } = message;
        const { fullName, ext } = WebFxScanUtility.pathParser(name);
        const fixedBase64 = WebFxScanUtility.base64Parser(base64, ext);
        // Execute autoScanCallback when get response
        if (typeof self.cache.autoScanCallback === "function") {
          self.cache.autoScanCallback({
            fileName: fullName,
            base64: fixedBase64,
            ocrText: recognizedata,
          });
        } else {
          // cache image for scan
          self.cache.scanIndex = self.cache.scanIndex + 1;
          self.cache.tempScanData.push({
            fileName: fullName,
            base64: fixedBase64,
            ocrText: recognizedata,
            md5,
          });
          // Execute scanCallback when get response
          if (typeof self.cache.scanCallback === "function") {
            // The imagefmt parameter will cause an additional return, containing the merged files.
            // Here, it's necessary to provide a status indicating that this file is a merged file rather than a scanned file, mainly for progress determination.
            const isMergeExist = ["pdf", "spdf", "ofd", "sofd", "tif"].includes(
              self.cache.scannerProperty?.imagefmt
            );
            const status =
              isMergeExist && ["pdf", "ofd", "tif"].includes(ext)
                ? "mergeFile"
                : "scanning";

            self.cache.scanCallback({
              fileName: fullName,
              base64: fixedBase64,
              ocrText: recognizedata,
              page: self.cache.scanIndex,
              status: status,
            });
          }
        }

        return;
      }
    } catch (error) {
      self.promiseReject(
        apiItem,
        WebFxScanUtility.failureResponse({
          errCode: "unknown",
          message: error,
          apiItem,
        })
      );
    }

    // unknown error code
  }

  // API: convert image type
  async convert(props) {
    const self = this;
    const { type, filename } = props;
    const command = JSON.stringify({
      "device-name": self.cache.scannerProperty["device-name"],
      source: self.cache.scannerProperty["source"],
      mode: "auto",
      "filename-format": `${filename}-0`,
      imagefmt: type,
    });

    const data = {
      filename,
      command,
    };
    return self.notify("convert", data);
  }
  callbackConvert(self, fixData) {
    const apiItem = this;
    try {
      const { type, func, data } = fixData;
      const { err_code, message } = data;
      const { name, base64 } = message;
      const { ext } = WebFxScanUtility.pathParser(name);
      if (err_code === 0) {
        self.promiseResolve(
          apiItem,
          WebFxScanUtility.successResponse({
            message: "OK",
            data: {
              base64: WebFxScanUtility.base64Parser(base64, ext),
              fileName: name,
            },
            apiItem,
          })
        );
      } else {
        self.promiseReject(
          apiItem,
          WebFxScanUtility.failureResponse({
            errCode: err_code,
            message,
            apiItem,
          })
        );
      }
    } catch (error) {
      self.promiseReject(
        apiItem,
        WebFxScanUtility.failureResponse({
          errCode: "unknown",
          message: error,
          apiItem,
        })
      );
    }
  }

  // API: merge multiple file to PDF
  async mergePdf(props) {
    const self = this;
    const { filelist } = props;
    const data = {
      base64enc: true,
      filelist,
    };
    return self.notify("mergePdf", data);
  }
  callbackMergePdf(self, fixData) {
    const apiItem = this;
    try {
      const { type, func, data } = fixData;
      const { err_code, message } = data;
      const { name, base64 } = message;
      const { ext } = WebFxScanUtility.pathParser(name);

      let fixName = name;
      if (name === "") {
        fixName = `MergePDF__${Date.now()}.pdf`;
      }

      if (err_code === 0) {
        self.promiseResolve(
          apiItem,
          WebFxScanUtility.successResponse({
            message: "OK",
            data: {
              base64: WebFxScanUtility.base64Parser(base64, ext),
              fileName: fixName,
            },
            apiItem,
          })
        );
      } else {
        self.promiseReject(
          apiItem,
          WebFxScanUtility.failureResponse({
            errCode: err_code,
            message,
            apiItem,
          })
        );
      }
    } catch (error) {
      self.promiseReject(
        apiItem,
        WebFxScanUtility.failureResponse({
          errCode: "unknown",
          message: error,
          apiItem,
        })
      );
    }
  }

  // API: image IP
  async imageProcess(props) {
    const self = this;
    const { scanParam = {}, filename = "" } = props;
    const { fullName, fileName, ext } = WebFxScanUtility.pathParser(filename);
    const command = JSON.stringify({
      "device-name": self.cache.scannerProperty["device-name"],
      source: self.cache.scannerProperty["source"],
      mode: "auto",
      "filename-format": `${fileName}-0`,
      ...scanParam,
    });
    const data = {
      filename,
      base64enc: true,
      command,
    };

    return self.notify("imageProcess", data);
  }
  callbackImageProcess(self, fixData) {
    const apiItem = this;
    try {
      const { type, func, data } = fixData;
      const { err_code, message } = data;
      const { name, base64 } = message;
      const { ext } = WebFxScanUtility.pathParser(name);

      if (err_code === 0) {
        self.promiseResolve(
          apiItem,
          WebFxScanUtility.successResponse({
            message: "OK",
            data: {
              base64: WebFxScanUtility.base64Parser(base64, ext),
              fileName: name,
            },
            apiItem,
          })
        );
      } else {
        self.promiseReject(
          apiItem,
          WebFxScanUtility.failureResponse({
            errCode: err_code,
            message,
            apiItem,
          })
        );
      }
    } catch (error) {
      self.promiseReject(
        apiItem,
        WebFxScanUtility.failureResponse({
          errCode: "unknown",
          message: error,
          apiItem,
        })
      );
    }
  }

  // API: delete all file on webFXScan server
  async deleteAll(props) {
    const self = this;
    return self.notify("deleteAll");
  }
  callbackDeleteAll(self, fixData) {
    const apiItem = this;
    try {
      const { type, func, data } = fixData;
      const { err_code, message } = data;

      if (err_code === 0) {
        self.promiseResolve(
          apiItem,
          WebFxScanUtility.successResponse({
            message: "OK",
            apiItem,
          })
        );
      } else {
        self.promiseReject(
          apiItem,
          WebFxScanUtility.failureResponse({
            errCode: err_code,
            message,
            apiItem,
          })
        );
      }
    } catch (error) {
      self.promiseReject(
        apiItem,
        WebFxScanUtility.failureResponse({
          errCode: "unknown",
          message: error,
          apiItem,
        })
      );
    }
  }

  // API: delete file on webFXScan server
  async deleteFile(props) {
    const self = this;
    const { filename = "" } = props;

    const data = {
      base64enc: true,
      filename,
    };
    return self.notify("deleteFile", data);
  }
  callbackDeleteFile(self, fixData) {
    const apiItem = this;
    try {
      const { type, func, data } = fixData;
      const { err_code, message } = data;

      if (err_code === 0) {
        self.promiseResolve(
          apiItem,
          WebFxScanUtility.successResponse({
            message: "OK",
            apiItem,
          })
        );
      } else {
        self.promiseReject(
          apiItem,
          WebFxScanUtility.failureResponse({
            errCode: err_code,
            message,
            apiItem,
          })
        );
      }
    } catch (error) {
      self.promiseReject(
        apiItem,
        WebFxScanUtility.failureResponse({
          errCode: "unknown",
          message: error,
          apiItem,
        })
      );
    }
  }
}

/* LIB Interface */
class WebFxScan {
  constructor(props) {
    const validatedProps =
      typeof props === "object" && !Array.isArray(props) && props !== null
        ? props
        : {};

    // single design
    if (!WebFxScan.instance) {
      WebFxScan.instance = this;
    }

    if (!this.serverInstance) {
      this.serverInstance = new WebFxScanServer(validatedProps);
    }

    return WebFxScan.instance;
  }
  serverInstance = null;

  connect(props) {
    const {
      ip = "",
      port = "",
      errorCallback = () => {},
      closeCallback = () => {},
    } = props;
    if (
      typeof errorCallback !== "function" ||
      typeof closeCallback !== "function"
    ) {
      WebFxScanUtility.failureResponse({
        errCode: "Parameter error",
        message:
          "errorCallback or closeCallback is not function. It require function type.",
      });
    }
    if (typeof ip !== "string" || typeof port !== "string") {
      WebFxScanUtility.failureResponse({
        errCode: "Parameter error",
        message: "ip or port is not string. It require string type.",
      });
    }

    return this.serverInstance.connect({
      ip,
      port,
      errorCallback,
      closeCallback,
    });
  }

  close() {
    return this.serverInstance.close();
  }

  init() {
    return this.serverInstance.init();
  }

  getDeviceList() {
    return this.serverInstance.getDeviceList();
  }

  getFileList() {
    return this.serverInstance.getFileList();
  }

  setScanner(props) {
    const { autoScan = false, callback = () => {}, ...scanParam } = props;
    if (typeof autoScan !== "boolean") {
      WebFxScanUtility.failureResponse({
        errCode: "Parameter error",
        message: "autoScan is not boolean. It require boolean type.",
      });
    }
    if (typeof callback !== "function") {
      WebFxScanUtility.failureResponse({
        errCode: "Parameter error",
        message: "callback is not function. It require function type.",
      });
    }
    return this.serverInstance.setProperty({ scanParam, autoScan, callback });
  }

  autoScan(callback) {
    return this.serverInstance.setProperty({ autoScan: true, callback });
  }

  scan(props = {}) {
    const { callback = null, timeout = null } = props;
    return this.serverInstance.scan({ callback, timeout });
  }

  convert(props) {
    const { filename = "", type = "" } = props;
    return this.serverInstance.convert({ filename, type });
  }

  exportPdf(filelist) {
    return this.serverInstance.mergePdf({ filelist });
  }

  rotate(props) {
    const { angle: rotate = 0, filename = "" } = props;
    return this.serverInstance.imageProcess({
      scanParam: { rotate },
      filename,
    });
  }

  deleteAll() {
    return this.serverInstance.deleteAll();
  }

  deleteFile(filename) {
    return this.serverInstance.deleteFile({ filename });
  }

  getVersion() {
    return this.serverInstance.version;
  }
}

if (typeof exports !== "undefined" && typeof module !== "undefined") {
  module.exports = WebFxScan;
}
