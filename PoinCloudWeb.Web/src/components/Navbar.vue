<template>
  <div class="navbar">
    <router-link class="navbar-caption" :to="'/'">PointCloudWeb</router-link>
    <div class="small-container">
      <router-link class="navbar-button" :to="'/About'">About</router-link>
      <router-link class="navbar-button" :to="'/scanner'">Scanner</router-link>
      <router-link class="navbar-button" :to="'/map'">Map</router-link>
    </div>
  </div>
</template>

<script>
export default {
  name: "NavBar",
};
</script>

<style scoped>
.navbar {
  padding: 10px;
  height: 2em;
  border-bottom: 1px solid rgb(161, 161, 161);
  color: rgb(145, 145, 145);
  display: flex;
  flex-direction: row-reverse;
  vertical-align: middle;
}
.navbar-caption {
  order: 99;
  margin-right: auto;
  font-size: 2em;
  display: table;
  vertical-align: middle;
}

.small-container {
  display: table;
  height: 2em;
}

.navbar-button {
  display: table-cell;
  width: 100px;
  font-size: 1em;
  vertical-align: middle;
}

a {
  font-weight: bold;
  color: #2c3e50;
  text-decoration: none;
}

a.router-link-exact-active {
  color: #42b983;
}
</style>