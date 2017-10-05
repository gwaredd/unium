const path = require('path');

const HtmlWebpackPlugin = require('html-webpack-plugin');
const HtmlWebpackPluginConfig = new HtmlWebpackPlugin({
  template: './src/www/index.html',
  filename: 'index.html',
  inject: 'body',
  favicon: 'src/www/favicon.ico'
})

module.exports = {
  entry: './src/www/index.jsx',
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
        test: /\.ico$/i,
        loader: 'file?name=[name].[ext]'
      }
    ]
  },

  plugins: [ HtmlWebpackPluginConfig ]
  
}
