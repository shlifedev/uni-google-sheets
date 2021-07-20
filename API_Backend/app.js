// shlifedev@gmail.com
var exports = exports || {};
var module = module || { exports: exports };
/*
 e = parameters
 type = http method (GET.. POST..) 
*/
function doProcessing(e, type) {
  return _____doProcessing(e, type);
}

function doGet(e) {
  return ContentService.createTextOutput(doProcessing(e, "GET"));
}

function doPost(e) {
  return ContentService.createTextOutput(doProcessing(e, "POST"));
}
