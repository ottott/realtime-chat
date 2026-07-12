import { createRouter, createWebHistory } from "vue-router";

import LoginView from "../views/LoginView.vue";
import RegisterView from "../views/RegisterView.vue";
import ChatView from "../views/ChatView.vue";
import { useAuthStore } from "../stores/auth";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: "/",
      redirect: "/login",
    },
    {
      path: "/login",
      component: LoginView,
    },
    {
      path: "/register",
      component: RegisterView,
    },
    {
      path: "/chat",
      component: ChatView,
      meta: {
                requiresAuth: true,
      },
    },
  ],
});


router.beforeEach((to) => {

    const auth = useAuthStore();

    if (to.meta.requiresAuth && !auth.token) {
        return "/login";
    }

});

export default router;