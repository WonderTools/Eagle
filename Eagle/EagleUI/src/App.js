import React from 'react'
import TestSuite from './components/TestSuite'
import testSuites from './containers/data.json';
import axios  from 'axios';

import './App.css'

export default class App extends React.Component {
  state = { testSuites : [] };

  refreshData = () =>{
    axios.get('https://localhost:6501/api/tests')
      .then(response=> {
        this.setState({testSuites : response.data});  
        console.log(response);
      })
    setTimeout(this.refreshData, 2000);
  }


  componentDidMount(){
    this.refreshData();
  }

  render(){
    const testSuites = this.state.testSuites;
    return(
      <div>
        {testSuites.map((t, index) => <TestSuite {...t} key={index}/>)}
      </div>
    )
  }
}