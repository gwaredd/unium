import Axios from 'axios'

export default {

  WIDGET: 'widget',

  DARK_COLOURS: {
    '#b80000': true,
    '#db3e00': true,
    '#fccb00': true,
    '#008b02': true,
    '#006b76': true,
    '#1273de': true,
    '#004dcf': true,
    '#5300eb': true,
  },

  Fetch: (url, appConfig) => {
    var uri = url.startsWith( "http" ) ? url : appConfig.api + url
    return Axios.get( uri )
  }
}

