import React, {Component } from 'react';
import TestCase from './TestCase';
import { FaChevronDown, FaChevronRight } from 'react-icons/fa';

const styles = {
   alignIcon : {
       verticalAlign:'middle'
   }
} 

export default class TestSuite extends Component {
    state = {
        isOpen: true
    }

    onToggle = isOpen => {
      this.setState({isOpen:!isOpen})
    }

    render(){
        const {name, testCases, testSuites} = this.props;
        const { isOpen } = this.state
        return (
            <div>
                <div onClick={()=>this.onToggle(isOpen)}>
                  { isOpen ? <FaChevronDown style={styles.alignIcon}/> : <FaChevronRight style={styles.alignIcon} /> } {name}
                </div>
                  {isOpen && 
                  <div style={{marginLeft:'40px'}}>
                     {testSuites.map((t, index) => <TestSuite {...t} key={index} />)}
                     {testCases.map((t, index) => <TestCase {...t} key={index} /> )}
                  </div>
                }
            </div>
        );
    } 
}