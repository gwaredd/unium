//-------------------------------------------------------------------------------

const _       = require( 'lodash' )
const path    = require( 'path' )
const webpack = require( 'webpack' )
const HtmlWebpackPlugin = require( 'html-webpack-plugin' )


//-------------------------------------------------------------------------------

module.exports = env => {
  
  const isDevServer = env.devserver || _.isUndefined( process.argv.find( v => v.includes('webpack-dev-server') ) ) == false

  return {

    entry: './src/index.jsx',
    
    output: {
      path: path.resolve('./root'),
      filename: 'index_bundle.js'
    },

    module: {
      loaders: [
        {
          test: /\.js$/,
          loader: 'babel-loader',
          exclude: /node_modules/
        },
        {
          test: /\.jsx$/,
          loader: 'babel-loader',
          exclude: /node_modules/
        },
        {
          test: /\.css$/,
          loader: "style-loader!css-loader"
        }
      ]
    },

    plugins: [

      new HtmlWebpackPlugin({
        template: './src/index.html',
        filename: 'index.html',
        inject: 'body'
      }),

      new webpack.DefinePlugin({
        DEVSERVER   : JSON.stringify( isDevServer ),
        PRODUCTION  : JSON.stringify( env.production ),
      })
    ],

    devServer: {
      headers: {
        "Access-Control-Allow-Origin": "*",
        "Access-Control-Allow-Methods": "GET, POST, PUT, DELETE, PATCH, OPTIONS",
        "Access-Control-Allow-Headers": "X-Requested-With, content-type, Authorization"
      }
    }  
  }
}

