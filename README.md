# JustAuth
JustAuth C#

# 如何使用

```csharp
// 第一步，创建 Client
var authConfig = new AuthConfig("id", "key", "redirectUrl");
var qqAuthRequest = new QQAuthRequest(authConfig);

// 第二步，生成授权链接
var state = Guid.NewGuid().ToString();
var url = qqAuthRequest.Authorize(state);

// 第三步，获取用户信息
// MVC 项目可通过 CallBackActionName([FromQuery] AuthCallback callback) 自动获取参数并实例化 callback 对象
var callback = new AuthCallback(); 
var userInfo = qqAuthRequest.Login(callback);
```

支持：QQ、GitHub

* [更多授权登录例子](https://github.com/leoskey/JustAuth/blob/master/Example/Controllers)
