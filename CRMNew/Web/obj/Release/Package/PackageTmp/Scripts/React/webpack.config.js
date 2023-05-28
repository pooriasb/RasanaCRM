module.exports={
  context:__dirname,
   //entry:"./app.js",
    entry: {
        //parent:"./Components/ParentComponent.jsx"
        
        factor: "./Components/factorcomponent.js",
        index:"./Components/index.js"
    },
  output:{
    path:__dirname +"/dist",
      filename:"[name]bundle.js"
    },
    mode: 'development',
  watch:true,
  module: {
      rules: [
          //...
          {
              test: /\.js$/,
              exclude:/(node_modules)/,
              type: 'javascript/auto',
              use: {
                  loader: 'babel-loader',
                  options: {
                      presets:['babel-preset-env','babel-preset-react']
                  }
              }
          }
      ]
    
    },
  //resolve: {
  //    extensions: ['.ts', '.tsx', '.js', '.jsx', '.json']
  //}
}