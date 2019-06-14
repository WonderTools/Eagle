import React, { Component } from 'react'
import SplitterLayout from 'react-splitter-layout'

import 'react-splitter-layout/lib/index.css'
import Tree from '../containers/Tree'

class HomeComponent extends Component {
    render() {
      return (
        <SplitterLayout>
          <div><Tree /></div>
          <div>Pane 2</div>
        </SplitterLayout>
      );
    }
  }
   
  export default HomeComponent;
