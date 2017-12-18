//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'

import AcModalScreenshot from '../dialogs/AcModalScreenshot.jsx'
import AcModalConfirm from '../dialogs/AcModalConfirm.jsx'
import AcModalAddTab from '../dialogs/AcModalAddTab.jsx'
import AcModalAddPanel from '../dialogs/AcModalAddPanel.jsx'
import AcModalAddWidget from '../dialogs/AcModalAddWidget.jsx'

const DialogComponents = {
  'screenshot': AcModalScreenshot,
  'confirm'   : AcModalConfirm,
  'addTab'    : AcModalAddTab,
  'addPanel'  : AcModalAddPanel,
  'addWidget' : AcModalAddWidget,
}

import { CancelDialog } from '../../actions/App.jsx'


//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    app: store.app
  }
})
export default class AcDialogs extends React.Component {

  onCancel = () => {
    this.props.dispatch( CancelDialog() )
  }
  

  render() {

    const { dialog, config } = this.props.app

    if( dialog == null || !(dialog.modal in DialogComponents) ) {
      return null
    }

    const $component = DialogComponents[ dialog.modal ]
    return <$component onCancel={ this.onCancel } dialog={ dialog } appConfig={ config } />
  }
}

