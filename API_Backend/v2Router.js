function _____doProcessing(e, type) {
  let password = "" 
  password = e.parameter.password;  
 
  if(password === null || password === undefined){
     const data = JSON.parse(e.postData.contents); 
     password = data.password;
  } 
  
  if (isPassValid(password)) {
    if (type == "GET") return get(e);
    if (type == "POST") return post(e);
  }
  throw new Error(`Password Invalid => ${password}`); 
}
