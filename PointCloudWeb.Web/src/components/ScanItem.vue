<template>
  <div>
    <font-awesome-icon class="icon" :icon="iconName" @click="onClickVisible"/>
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
            <span class="button-text">Delete</span>
          </button>

          <p class="data-caption">Rotation</p>

          <input class="input-num" type="number" v-model="editRotationX" @keyup.enter="onEnter()"/>
          <input class="input-num" type="number" v-model="editRotationY" @keyup.enter="onEnter()"/>
          <input class="input-num" type="number" v-model="editRotationZ" @keyup.enter="onEnter()"/>

          <p class="data-caption">Transformation</p>
          <input class="input-num" type="number" v-model="editTransformationX" @keyup.enter="onEnter()"/>
          <input class="input-num" type="number" v-model="editTransformationY" @keyup.enter="onEnter()"/>
          <input class="input-num" type="number" v-model="editTransformationZ" @keyup.enter="onEnter()"/>


          <button @click="onClickSave()">
            <font-awesome-icon class="icon" icon="edit"></font-awesome-icon>
            <span class="button-text">Save</span>
          </button>
          <button @click="onClickEdit()">
            <font-awesome-icon class="icon" icon="times"></font-awesome-icon>
            <span class="button-text">Cancel</span>
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
      editRotationX: 0,
      editRotationY: 0,
      editRotationZ: 0,
      editTransformationX: 0,
      editTransformationY: 0,
      editTransformationZ: 0,
    };
  },
  methods: {
    onClickVisible() {
      this.isVisible = !this.isVisible;
      this.$store.dispatch("pci/updateVisible", {
        id: this.item.id,
        visible: this.isVisible,
      });
    },
    onEnter() {
      this.onClickSave();
    },
    onClickEdit() {
      this.isCollapsed = !this.isCollapsed;

      if (this.isCollapsed) {
        this.$refs.settings.style.height = "0";
      } else {
        this.editPcName = this.item.name;
        this.editRotationX = this.item.rotation.x;
        this.editRotationY = this.item.rotation.y;
        this.editRotationZ = this.item.rotation.z;
        this.editTransformationX = this.item.transformation.x;
        this.editTransformationY = this.item.transformation.y;
        this.editTransformationZ = this.item.transformation.z;
        this.$refs.settings.style.height =
            this.outerHeight(this.$refs["settings-container"]) + "px";
        setTimeout(() => this.$refs.focusElement.focus(), 100);
      }
    },
    onClickSave() {
      this.$store.dispatch("pci/updatePointCloud", {
        id: this.item.id,
        name: this.editPcName,
        rotation: {
          x: parseFloat(this.editRotationX),
          y: parseFloat(this.editRotationY),
          z: parseFloat(this.editRotationZ)
        },
        transformation: {
          x: parseFloat(this.editTransformationX),
          y: parseFloat(this.editTransformationY),
          z: parseFloat(this.editTransformationZ)
        }
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
      let height = el.offsetHeight;
      const style = getComputedStyle(el);

      height += parseInt(style.marginTop) + parseInt(style.marginTop);
      return height;
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
  margin: 0 0 0 5px;
  font-size: 0.8em;
  cursor: pointer;
  user-select: none;
}

.button-delete {
  display: inline-block;
  margin-left: 5px;
}

.input-num {
  display: inline-block;
  width: 60px;
}

.button-text {
  display: inline-block;
  margin: 0;
}

#settings {
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

.data-caption {
  font-size: 0.5em;
  margin-bottom: 0px;

}
</style>
