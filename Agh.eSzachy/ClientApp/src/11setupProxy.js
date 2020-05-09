const { createProxyMiddleware } = require("http-proxy-middleware");
module.exports = function (app) {
  app.use(
    "/.well-known",
    createProxyMiddleware({
      target: "https://agheszachy20200308103908.azurewebsites.net/.well-known",
    })
  );

  app.use(
    "/_configuration",
    createProxyMiddleware({
      target:
        "https://agheszachy20200308103908.azurewebsites.net/_configuration",
    })
  );
};
