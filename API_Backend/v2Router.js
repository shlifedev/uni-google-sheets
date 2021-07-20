function _____doProcessing(e, type) {
  if (isPassValid(e.parameter.password)) {
    if (type == "GET") return get(e);
    if (type == "POST") return post(e);
  }
  return json({ message: " ugs password is invalid " });
}
