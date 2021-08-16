<template>
  <div class="ui">
    <h1>Welcome to PointCloudWeb</h1>
    <div v-if="connection_status && !scan_status && scanner_status">
      <button class="button" v-on:click="sendMessage('<start><0>')" >Start Scan: low</button>
      <button class="button button2" v-on:click="sendMessage('<start><1>')" >Start Scan: medium</button>
      <button class="button button3" v-on:click="sendMessage('<start><2>')" >Start Scan: high</button>
    </div>
      <button v-if="scan_status && connection_status" class="button button4" v-on:click="sendMessage('<stop>')" >cancel</button>
      <button v-if="!scanner_status && connection_status" class="button button4" v-on:click="sendMessage('<init>')" >Init. Scanner</button>
    <div class="progressbar" v-if="connection_status && scanner_status">
      <div :style="{width: progress + '%', 'background-color': progress_color}"></div>
    </div>
    <p v-if="connection_status && scanner_status">{{progress}} %</p>
    <div>
      <h1 v-if="!connection_status">Scan Server: disconnected</h1>
      <h1 v-if="connection_status">Scan Server: connected</h1>
      <button  v-if="!connection_status" v-on:click="connectWS">Connect to Scan Server</button>
      <ul>
        <div class="value" v-for="(item, index) in logs" :key="item.id">
            <li>{{logs[index]}}</li>
        </div>
      </ul>
    </div>
      <!--<button v-on:click="connection_status = !connection_status" >test</button>-->
      <button v-on:click="progress = 0, logs = []" >clear logs</button>
  </div>
</template>

<script>
export default {
  data() {
    return {
      wsConnection: null,
      logs: [],
      connection_status: false,
      scan_status: false,
      scanner_status: false,
      progress: 0,
      progress_color: "yellow",
      command: "",
      value: ""
    };
  },
  methods: {
    sendMessage(message) {
      this.wsConnection.send(message);
    },
    connectWS(){
      this.wsConnection = new WebSocket("ws://127.0.0.1:6789/")
      let that = this
      
      this.wsConnection.onopen = function(){
        that.connection_status = true
      }

      this.wsConnection.onmessage = function(event){
        if(event.data)
        that.msgFilter(event.data)
      }

      this.wsConnection.onclose = function(){
        that.connection_status = false
        that.scanner_status = false
        that.scan_status = false
        that.logs.push("Websocket Connection closed");
      }
    },
    msgFilter(message){
      let that = this
      if(message.search("<") != -1){
        that.command = message.substr(message.search("<")+1, message.search(">")-1)
        that.value = message.substr(message.search(">")+1)
        //console.log("command: " + that.command + " / value: " + that.value)
        this.action(that.command, that.value)
      }
      else{
        that.command = "log"
        that.value = message
        this.action(that.command, that.value)
      }
    },
    action(command, value){
      let that = this
      if(command == "progress"){
        that.progress = parseInt(value, 10)
      }
      else if(command == "log"){
        that.logs.push(value);
      }
      else if(command == "scan"){
        if(value == "running"){
          that.scan_status = true
          that.progress_color = "yellow"
        }
        else if(value == "finished"){
          that.scan_status = false
          that.progress_color = "greenyellow"
        }
        else{
          that.scan_status = false
          that.progress_color = "red"
          that.progress = "canceld"
        }
      }
      else if(command == "connection"){
        if(value == "true")
          that.scanner_status = true
        else
          that.scanner_status = false
          that.scan_status = false
      }
      else
        that.logs.push("Unknow command: " + value)

    }
  },
  created() {
    this.connectWS();
  }
};
</script>

<style scoped>

.ui {
  width: 600px;
  margin: auto;
}

ul {
  overflow: scroll;
  background: lightgray;
  height: 200px;
}

li {
  list-style-type: none;
  margin: 0;
  padding: 0;
}

.progressbar {
  background-color: grey;
  border-radius: 7px;
  /* (height of inner div) / 2 + padding */
  padding: 3px;
  margin: auto;
}

.progressbar>div {
  /* Adjust with JavaScript */
  height: 20px;
  border-radius: 4px;
}

.button {
  background-color: #4CAF50; /* Green */
  border: none;
  color: white;
  padding: 15px 32px;
  text-align: center;
  text-decoration: none;
  display: inline-block;
  font-size: 16px;
  margin: 4px 2px;
  cursor: pointer;
}

.button:hover{
  background-color: #e9962a;
}

.button2 {background-color: #00ba9b;}
.button3 {background-color: #008cff}
.button4 {background-color: #f44336;} /* Red */ 

</style>