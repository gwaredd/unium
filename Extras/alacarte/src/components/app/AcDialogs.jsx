//-------------------------------------------------------------------------------

import React, {Fragment} from 'react'
import { connect } from 'react-redux'

import AcModalScreenshot from '../dialogs/AcModalScreenshot'
import AcModalConfirm from '../dialogs/AcModalConfirm'
import AcModalAddTab from '../dialogs/AcModalAddTab'
import AcModalAddPanel from '../dialogs/AcModalAddPanel'
import AcModalAddWidget from '../dialogs/AcModalAddWidget'

import { CancelDialog } from '../../actions/App'

const DialogComponents = {
  'screenshot': AcModalScreenshot,
  'confirm'   : AcModalConfirm,
  'addTab'    : AcModalAddTab,
  'addPanel'  : AcModalAddPanel,
  'addWidget' : AcModalAddWidget,
}

//-------------------------------------------------------------------------------

class AcDialogs extends React.Component {

  onCancel = () => {
    this.props.dispatch( CancelDialog() )
  }

  render() {

    const { dialog, config } = this.props.app

    if( dialog === null || !(dialog.modal in DialogComponents) ) {
      return null
    }

    const component = DialogComponents[ dialog.modal ]
    const element = React.createElement( component, {
      onCancel: this.onCancel,
      dialog: dialog,
      appConfig: config
    })

    return (
      <Fragment>
        {element}
      </Fragment>
    )
  }
}



const mapStateToProps = ( state, ownProps ) => {
  return {
    app: state.app
  }
}

export default connect( mapStateToProps )( AcDialogs );

