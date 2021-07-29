<template>
  <div>
    <font-awesome-icon class="icon" :icon="iconName" @click="onClickVisible" />
    <font-awesome-icon class="icon" icon="edit" @click="onClickEdit" />
    <p>Scan Name</p>
    <!--     
    button(id="Visible" + scan.id, onclick="buttonVisibleClicked("+scan.id+")"type="button", class="fa fa-eye") 
    button(id="Edit" + scan.id, type="button",class="fa fa-edit") 
    p(id="Label" + scan.id)= scan.name -->
  </div>
</template>

<script>
// import MenuIcon from "vue-material-design-icons/Menu.vue";

export default {
  name: "ScanItem",
  data: function () {
    return {
      visible: true,
    };
  },
  methods: {
    onClickVisible: function () {
      this.visible = !this.visible;
    },
    onClickEdit: function () {
      alert("Edit!");
    },
  },
  computed: {
    iconName: function () {
      if (this.visible) return "eye";
      else return "eye-slash";
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
}
</style>e