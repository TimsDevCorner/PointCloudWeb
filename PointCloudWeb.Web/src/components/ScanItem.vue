<template>
  <div>
    <font-awesome-icon class="icon" :icon="iconName" @click="onClickVisible" />
    <p class="caption" @click="onClickEdit()">{{ item.name }}</p>
    <div id="settings" ref="settings" class="collapsed">
      <div id="settings-container" ref="settings-container">
        <div>
          <input
            ref="focusElement"
            type="text"
            v-model="editPcName"
            @keyup.enter="onEnter()"
          />

          <button class="button-delete" @click="onClickDelete()">
            <font-awesome-icon class="icon" icon="trash"></font-awesome-icon>
            <p class="button-text">Delete</p>
          </button>

          <button @click="onClickSave()">
            <font-awesome-icon class="icon" icon="edit"></font-awesome-icon>
            <p class="button-text">Save</p>
          </button>
          <button @click="onClickEdit()">
            <font-awesome-icon class="icon" icon="times"></font-awesome-icon>
            <p class="button-text">Cancel</p>
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
      editPcName: "",
    };
  },
  methods: {
    onClickVisible() {
      this.isVisible = !this.isVisible;
    },
    onEnter() {
      this.onClickSave();
    },
    onClickEdit() {
      this.isCollapsed = !this.isCollapsed;

      if (this.isCollapsed) {
        this.$refs.settings.style.height = 0;
      } else {
        this.editPcName = this.item.name;
        this.$refs.settings.style.height =
          this.outerHeight(this.$refs["settings-container"]) + "px";
        setTimeout(() => this.$refs.focusElement.focus(), 100);
      }
    },
    onClickSave() {
      this.$store.dispatch("pci/updatePointCloud", {
        id: this.item.id,
        name: this.editPcName,
      });
      this.onClickEdit();
    },
    onClickDelete() {
      this.$store.dispatch("pci/deletePointCloud", {
        id: this.item.id,
        name: this.editPcName,
      });
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

.caption {
  display: inline-block;
  margin: 0px;
  margin-left: 5px;
  font-size: 0.8em;
  cursor: pointer;
  user-select: none;
}

.button-delete {
  display: inline-block;
  margin-left: 10px;
}

.button-text {
  display: inline-block;
  margin: 0px;
}

#settings {
  -moz-transition: height 0.1s;
  -ms-transition: height 0.1s;
  -o-transition: height 0.1s;
  -webkit-transition: height 0.1s;
  transition: height 0.1s;
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
</style>
