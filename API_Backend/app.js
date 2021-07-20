// shlifedev@gmail.com
var exports = exports || {};
var module = module || { exports: exports };
/*
 e = parameters
 type = http method (GET.. POST..) 
*/

function getInstruction(e, type){ 
   if(type == "POST") 
   {
     
     
     const data= JSON.parse(e.postData.contents); 
     return data.instruction;
   }
   if(type == "GET") 
     return e.parameter.instruction;
  
   return null;
}




function doProcessing(e, type) {
  try 
  {
    return _____doProcessing(e, type);
  }
  catch (err) {
    const instruction = getInstruction(e,type);
    const errorRes = {
      instruction,
      error : {
      message: err.message,
      eReq: e,
      eType: type,
      eStackTrace: err.stack, 
      }
    } 
    return json(errorRes);
  }
}

function doGet(e) {
  return ContentService.createTextOutput(doProcessing(e, "GET"));
}

function doPost(e) {
  return ContentService.createTextOutput(doProcessing(e, "POST"));
}
