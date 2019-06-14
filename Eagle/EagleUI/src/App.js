import React from 'react'
import TestSuite from './components/TestSuite'
import testSuites from './containers/data.json';

import './App.css'

export default class App extends React.Component {
  render(){
    return(
      <div>
        {testSuites.map((t, index) => <TestSuite {...t} key={index}/>)}
      </div>
    )
  }
}