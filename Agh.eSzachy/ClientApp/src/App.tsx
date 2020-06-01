import React, { Component } from "react";
import { Route } from "react-router";

import { Layout } from "./components/Layout";
import { Home } from "./components/Home";

import AuthorizeRoute from "./components/api-authorization/AuthorizeRoute";
import ApiAuthorizationRoutes from "./components/api-authorization/ApiAuthorizationRoutes";
import { ApplicationPaths } from "./components/api-authorization/ApiAuthorizationConstants";

import { Game } from "./components/Game";

import "./custom.css";
class App extends Component {
  static displayName = App.name;

  render = () => (
    <Layout>
      <AuthorizeRoute exact path="/" component={Home} />
      <AuthorizeRoute path="/room/:name" component={Game} />

      <Route
        path={ApplicationPaths.ApiAuthorizationPrefix}
        component={ApiAuthorizationRoutes}
      />
    </Layout>
  );
}

export default App;
