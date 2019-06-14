import React, {Component } from 'react';
import TestCase from './TestCase';
import { FaChevronDown, FaChevronRight } from 'react-icons/fa';
import { ContextMenu, MenuItem, ContextMenuTrigger } from "react-contextmenu";
import axios from 'axios';

const styles = {
   alignIcon : {
       verticalAlign:'middle'
   }
} 


export default class TestSuite extends Component {
    state = {
        isOpen: true
    }

    handleClick =(e, data)=> {
    
      console.log(data.foo);
    }
  
    scheduleTest =(e, data1)=> {
      console.log(data1);
      const options = {
        method: 'POST',
        headers: { 'content-type': 'application/json' },
        data: JSON.stringify(data1.id),
        url: "https://localhost:6501/api/tests/schedule",
      };
      axios(options)
      .then(response =>{
        console.log(response);
      });
    }

    onToggle = isOpen => {
      this.setState({isOpen:!isOpen})
    }

    render(){
        const {name, id, testCases, testSuites} = this.props;
        const { isOpen } = this.state
        return (
            <div>
                <div>
                
                <ContextMenuTrigger id={'menu_id'+ id} >
                <div><span onClick={()=>this.onToggle(isOpen)}>{ isOpen ? <FaChevronDown style={styles.alignIcon}/> : <FaChevronRight style={styles.alignIcon} /> }</span>{name}</div>
                </ContextMenuTrigger>
                <ContextMenu id={'menu_id'+ id}  className="menu">
                  <MenuItem onClick={this.scheduleTest} data={{ id }}>Schedule Test</MenuItem>
                  <MenuItem onClick={this.handleClick} data={{ item: 'item 2' }}>Show History</MenuItem>
                </ContextMenu>
                
                </div>
                  {isOpen && 
                  <div style={{marginLeft:'20px'}}>
                     {testSuites.map((t, index) => <TestSuite {...t} key={index} />)}
                     {testCases.map((t, index) => <TestCase {...t} key={index} /> )}
                  </div>
                }
            </div>
        );
    } 
}