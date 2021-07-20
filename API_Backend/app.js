const instructions = {
  search_google_drive : 0,
  read_spreadsheet : 1,
  write_object : 2,
  create_default_table : 101,
  copy_example : 102
} 
// shlifedev@gmail.com 
var exports = exports || {};
var module = module || { exports: exports }; 
/*
 e = parameters
 type = http method (GET.. POST..) 
*/
function doProcessing(e, type) {  
  
  return _____doProcessing(e,type);
  
  if(type == "GET") 
  {    
     if(e.parameter.password != null && isPassValid(e.parameter.password))
     { 
        switch (e.parameter.instruction)
        {
           case "getTable": // get sheet datas
             return sheetToJson(e.parameter.sheetID);
           case "getFolderInfo":
             return getFolderInfo(e.parameter.folderID, null);
           case "copyExampleSheets":
             return copyExampleSheets(e.parameter.folderID);  
        } 
    }
    else
    {
       var data = {};
       data.result = "invalid apssword or some error";
       return JSON.stringify(data);  
    }
  }


  if(type == "POST")
  {
     var data = JSON.parse(e.postData.contents); 
     if(data.password != null && isPassValid(data.password))
     {
       switch (data.instruction)
       {
         case "writeData": // get sheet datas
           return writeData(data.spreadSheetID,data.sheetID,data.key,data.value);  
         case "createDefaultTable": // get sheet datas
           return createDefaultTable(data.folderID, data.fileName);
       } 
       var data = {};
       data.result = 2;
       return JSON.stringify(data);
     }
     else
     {
       var data = {};
       data.result = 2;
       return JSON.stringify(data); 
     }
   }
    return e.parameter.instruction;
}  
function doGet(e) { 
    return ContentService.createTextOutput(doProcessing(e, "GET"));
} 

function doPost(e)
{ 
  return ContentService.createTextOutput(doProcessing(e, "POST"));
}