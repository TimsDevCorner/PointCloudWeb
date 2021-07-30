<template>
  <div>
    <font-awesome-icon class="icon" :icon="iconName" @click="onClickVisible" />
    <p @click="onClickEdit()">{{ item.name }}</p>
    <div id="settings" ref="settings" class="collapsed">
      <div id="settings-container" ref="settings-container">
        <div>
          <input type="text" value="Scan Name" />

          <button>
            <font-awesome-icon class="icon" icon="edit"></font-awesome-icon>
            <p>Save</p>
          </button>
          <button @click="onClickEdit()">
            <font-awesome-icon class="icon" icon="edit"></font-awesome-icon>
            <p>Cancel</p>
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "ScanItem",
  props: ["item"],
  data() {
    return {
      isVisible: true,
      isCollapsed: true,
    };
  },
  methods: {
    onClickVisible() {
      this.isVisible = !this.isVisible;
    },
    onClickEdit() {
      this.isCollapsed = !this.isCollapsed;

      if (this.isCollapsed) {
        this.$refs.settings.style.height = 0;
      } else {
        this.$refs.settings.style.height =
          this.outerHeight(this.$refs["settings-container"]) + "px";
      }
    },
    outerHeight(el) {
      var width = el.offsetHeight;
      const style = getComputedStyle(el);

      width += parseInt(style.marginTop) + parseInt(style.marginTop);
      return width;
    },
  },
  computed: {
    iconName() {
      return this.isVisible ? "eye" : "eye-slash";
    },
  },
};
</script>

<style scoped>
.icon {
  margin-right: 5px;
  width: 25px;
  cursor: pointer;
}

p {
  display: inline-block;
  margin: 0px;
  margin-left: 5px;
  font-size: 0.8em;
  cursor: pointer;
  user-select: none;
}

#settings {
  -moz-transition: height 0.3s;
  -ms-transition: height 0.3s;
  -o-transition: height 0.3s;
  -webkit-transition: height 0.3s;
  transition: height 0.3s;
  height: 0;
  overflow: hidden;
}

#settings-container {
  padding: 10px;

  margin-left: 25px;
  margin-top: 10px;
  margin-bottom: 30px;
  border-style: solid;
  border-width: 1px;
  border-color: grey;
}
</style>e