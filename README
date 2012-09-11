Yubico dotnet client
--------------------

Validation protocol 2.0 client for dotNet.
To use this with YubiCloud you will need a free clientId and apiKey from https://upgrade.yubico.com/getapikey/

Usage
-----

YubicoClient client = new YubicoClient(clientId, apiKey);
YubicoResponse response = client.Verify(otp);
if(response.Status == YubicoResponseStatus.Ok) 
{
  // validation success
} 
else 
{
  // validation failure
}
