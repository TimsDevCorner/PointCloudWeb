<template>
  <div class="about">
    <img alt="Vue logo" src="../assets/logo.png" />
    <HelloWorld msg="Welcome to Your Vue.js App" />
  </div>
</template>

<script>
import HelloWorld from "@/components/HelloWorld.vue";
// @ is an alias to /src

export default {
  name: "Home",
  components: {
    HelloWorld,
  },
};
</script>
