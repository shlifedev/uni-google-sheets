// shlifedev@gmail.com
var exports = exports || {};
var module = module || { exports: exports };
/*
 e = parameters
 type = http method (GET.. POST..) 
*/
function doProcessing(e, type) {
  try {
    return _____doProcessing(e, type);
  } catch (err) {
    return json({
      message: err.message,
      eReq: e,
      eType: type,
    });
  }
}

function doGet(e) {
  return ContentService.createTextOutput(doProcessing(e, "GET"));
}

function doPost(e) {
  return ContentService.createTextOutput(doProcessing(e, "POST"));
}
