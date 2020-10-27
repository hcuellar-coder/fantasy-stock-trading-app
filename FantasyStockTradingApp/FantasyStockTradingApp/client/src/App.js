import React from 'react';
import Home from './Components/Home';
import Summary from './Components/Summary';
import Report from './Components/Report';
import Layout from './Components/Layout';
import { Route } from 'react-router';
import './App.css';


function App() {
  return (
    <div>
      <Layout >
        <Route exact path='/' component={Home} />
        <Route path='/summary' component={Summary} />
        <Route path='/report' component={Report} />
      </Layout>
    </div>
  );
}

export default App;
