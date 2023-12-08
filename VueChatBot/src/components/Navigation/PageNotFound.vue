<template>
  <div class="container">
    <div class="row">
      <div class="col-md-12">
        <div class="error-template">
          <h1>Oops!</h1>
          <h2>404 Not Found</h2>
          <div class="error-details">
            Sorry, an error has occured, Requested page not found!
          </div>
          <div class="error-actions d-flex">
            <router-link to="/login" class="ms-0">
              <v-btn flat elevation="0" color="success darken-2"
              ><v-icon>mdi-home</v-icon> Take Me Home</v-btn
              >
            </router-link>

            <v-btn flat elevation="0" append class="ms-2" color="success darken-2"
            ><v-icon>mdi-email</v-icon>Contact Support</v-btn
            >
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "PageNotFound",
  methods: {
    redirectToHome: function () {
      this.$router.push("/login");
    },
  },
};
</script>
