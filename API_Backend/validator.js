 
///Auth
function isPassValid(password)
{ 
  var appPass = PropertiesService.getScriptProperties().getProperty("password");
  if (password != null && password == appPass)
      return true;
  
  return false;
}  

function isWriteable(){
    var data = PropertiesService.getScriptProperties().getProperty("write");
    if (data !== null && data !== undefined && data === "true") 
      return true;
  
  return false;
}