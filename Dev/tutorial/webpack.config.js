//--------------------------------------------------------------------------------

var webpack = require( 'webpack' )
var path    = require( 'path' )
var HtmlWebpackPlugin = require( 'html-webpack-plugin' )

//--------------------------------------------------------------------------------

var BUILD_DIR   = path.resolve( __dirname, 'dist' )
var APP_DIR     = path.resolve( __dirname, 'src' )

var config = {
  entry: APP_DIR + '/app.jsx',
  output: {
    path: BUILD_DIR,
    filename: 'tutorial.jsx'
  },
  plugins: [ new HtmlWebpackPlugin( { template:'./src/index.html' } ) ],
  module : {
    loaders : [
      {
        test : /\.jsx?/,
        include : APP_DIR,
        loader : 'babel-loader'
      },
      { 
        test: /\.css$/, 
        loader: "style-loader!css-loader" 
      },
      {
        test: /\.(woff|woff2|ttf|eot|svg)(\?v=\d+\.\d+\.\d+)?$/, 
        loader: 'url-loader?limit=131072&mimetype=application/font-woff'
      },
      {
        test: /\.ico$/,
        loader: 'file-loader?name=[name].[ext]'
      }
    ]
  }
}

module.exports = config

