<template>
  <div>
    <h1>Welcome to PointCloudWeb</h1>
    <div class="scanresolution">
      <button v-on:click="sendMessage('<start><0>')" >Start Scan: low</button>
      <button v-on:click="sendMessage('<start><1>')" >Start Scan: medium</button>
      <button v-on:click="sendMessage('<start><2>')" >Start Scan: high</button>
      <button v-on:click="sendMessage('Connect')" >Connect</button>
    </div>
    <div class="scanresolution">
      <h1>status: {{status}}</h1>
      <div class="value" v-for="(item, index) in logs" :key="item.id">
          {{logs[index]}}
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      logs: [],
      status: "disconnected"
    };
  },
  methods: {
    sendMessage: function(message) {

      this.connection.send(message);
    }
  },
  created: function() {
    this.connection = new WebSocket("ws://127.0.0.1:6789/")
    let that = this

    this.connection.onopen = function(){
      that.status = "connected"
      that.logs.push("successfully connected to scanserver")
    }

    this.connection.onmessage = function(event){
      console.log(event)
      that.logs.push(event.data);
    }

  }
};
</script>

<style scoped>

</style>