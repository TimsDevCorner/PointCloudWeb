<template>
  <div class="map-grid-container">
    <div id="map-settings">
      <div class="map-settings-internal">
        <h3>Point Clouds</h3>

        <ul v-for="item in cloudItems" :key="item">
          <li><ScanItem :item="item" /></li>
        </ul>
      </div>
    </div>
    <div id="map" class="content-padding">
      <h1>Place the Point Cloud visualization here</h1>
    </div>
  </div>
</template>
<script>
import ScanItem from "@/components/ScanItem";

export default {
  created() {
    this.$store.dispatch("pci/loadPointClouds");
  },
  computed: {
    cloudItems() {
      return this.$store.state.pci.pointClouds;
    },
  },
  components: { ScanItem },
};
</script>

<style scoped>
.map-grid-container {
  text-align: left;
}
.content-padding {
  padding: 10px;
}

ul {
  padding-left: 0px;
}

li {
  font-size: 1.4em;
  list-style: none;
}

p {
  display: inline-block;
  margin: 0px;
  margin-left: 10px;
}

h3 {
  margin: 0px;
}

a {
  text-decoration: none;
}

button {
  display: inline-block;
  border-top: 0px;
  border-bottom: 0px;
  border-left: 0px;
  border-right: 0px;

  background: none;
  cursor: pointer;
  color: grey;
  font-size: inherit;
}

#map-settings {
  grid-area: right;
  border-left: 1px solid black;
  height: 100%;
  overflow-y: auto;
}
.map-settings-internal {
  padding: 20px;
}

#map {
  grid-area: main;
  overflow-y: auto;
}

.map-grid-container {
  display: grid;
  grid-template-areas: "main right";
  grid-template-columns: calc(100vw - 325px) 325px;
  height: calc(
    100vh - 2em - 20px - 1px
  ); /*viewport height - height of navbar-button - padding - borderline*/
  grid-gap: 0px;
  padding: 0px;
}
</style>