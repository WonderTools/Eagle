import React, { Component } from 'react'
import { FaCheck, FaTimes, FaQuestion, FaExclamationTriangle } from 'react-icons/fa';
import { ContextMenu, MenuItem, ContextMenuTrigger } from "react-contextmenu"
import axios from 'axios';

const getTestResultIcon = result => {
  if(result === 'pass')
    return <FaCheck style={{color:'green'}} />
  if(result === 'fail')
    return <FaTimes style={{color:'red'}} />
  if(result === 'Inconclusive')
    return <FaQuestion style={{color:'yellow'}} />
  if(result === null)
    return <FaExclamationTriangle style={{color:'grey'}} />
}

export default class TestCase extends Component {

  constructor(props) {
    super(props);

    this.state = { logs: [] };
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

  render() {
    const { id, name, result, finishingTime, startingTime, durationInMs } = this.props
    return(
      <div>
      <ContextMenuTrigger id={'menu_id'+ id} >
        {getTestResultIcon(result)} {name}
      </ContextMenuTrigger>
    
      <ContextMenu id={'menu_id'+ id}  className="menu">
        <MenuItem onClick={this.scheduleTest} data={{ id }}>Schedule Test</MenuItem>
        <MenuItem onClick={this.handleClick} data={{ item: 'item 2' }}>Show History</MenuItem>
      </ContextMenu>
    </div>
    )
  }
}