<template>
  <div>
    <font-awesome-icon class="icon" :icon="iconName" @click="onClickVisible" />
    <font-awesome-icon class="icon" icon="edit" @click="onClickEdit" />
    <p @click="onClickEdit()">Scan Name</p>
    <div id="settings" class="collapsed">
      <div id="settings-container">
        <div>
          <input type="text" value="Scan Name" />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "ScanItem",
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

      const settings = document.getElementById("settings");
      if (this.isCollapsed) {
        settings.style.height = 0;
      } else {
        const container = document.getElementById("settings-container");
        settings.style.height = this.outerHeight(container) + "px";
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