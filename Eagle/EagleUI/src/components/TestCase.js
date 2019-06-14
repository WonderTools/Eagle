import React, { Component } from 'react'
import { FaCheck, FaTimes, FaQuestion, FaExclamationTriangle } from 'react-icons/fa';
import { ContextMenu, MenuItem, ContextMenuTrigger } from "react-contextmenu"

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

  render() {
    const { id, name, result, finishingTime, startingTime, durationInMs } = this.props
    return(
      <div>
      <ContextMenuTrigger id={'menu_id'+ id} >
        {getTestResultIcon(result)} {name}
      </ContextMenuTrigger>
    
      <ContextMenu id={'menu_id'+ id}  className="menu">
        <MenuItem onClick={this.handleClick} data={{ item: 'item 1' }}>Menu Item 1</MenuItem>
        <MenuItem onClick={this.handleClick} data={{ item: 'item 2' }}>Menu Item 2</MenuItem>
        <MenuItem divider />
        <MenuItem onClick={this.handleClick} data={{ item: 'item 3' }}>Menu Item 3</MenuItem>
      </ContextMenu>
    </div>
    )
  }
}