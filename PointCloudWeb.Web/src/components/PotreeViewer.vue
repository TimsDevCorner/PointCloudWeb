<template>
  <div class="full-size">
    <!--    <p>{{ scans === null ? scans[0].name : "" }}</p>-->
    <iframe v-on:load="onLoadIframe" name="myIframe" class="full-size" ref="viewer"
            src="Potree/examples/pcw.html"></iframe>
  </div>
</template>

<script>
export default {
  name: "PotreeViewer",
  data() {
    return {
      oldScans: [],
      iframe: null
    }
  },
  computed: {
    scans() {
      return this.$store.state.pci.pointClouds
    }
  },
  watch: {
    scans: {
      handler: function () {
        this.pointCloudsChanged();
      },
      deep: true //To receive changes in array-objects, like name
    }
  },
  methods: {
    pointCloudsChanged() {
      if (this.iframe !== null) {
        this.iframe.window.loadFromArray(this.scans);
      }
    },
    onLoadIframe() {
      for (let i = 0; i < window.frames.length; i++) {
        if (window.frames[i].name === "myIframe") {
          this.iframe = window.frames[i];
          break;
        }
      }

      this.pointCloudsChanged();
    }
  }
}
</script>

<style scoped>

.full-size {
  width: 100%;
  height: Calc(100% - 5px);
  border: 0;
}

</style>