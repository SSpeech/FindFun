import { createRouter, createWebHistory } from 'vue-router'
import { RoutePaths, Routes } from '@/config/Enums'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: RoutePaths.Home,
      name: Routes.Home,
      component: () => import('../views/HomeView.vue'),
    },
    {
      path: RoutePaths.About,
      name: Routes.About,
      component: () => import('../views/AboutView.vue'),
    },
    {
      path: RoutePaths.Events,
      name: Routes.Events,
      component: () => import('../views/EventsView.vue'),
    },
    {
      path: RoutePaths.EventDetail,
      name: Routes.EventDetail,
      component: () => import('../views/DetailView.vue'),
      props: true,
    },
    {
      path: RoutePaths.Parks,
      name: Routes.Parks,
      component: () => import('../views/ParksView.vue'),
    },
    {
      path: RoutePaths.ParkDetail,
      name: Routes.ParkDetail,
      component: () => import('../views/DetailView.vue'),
      props: true,
    },
    {
      path: RoutePaths.Login,
      name: Routes.Login,
      component: () => import('../views/LoginView.vue'),
    },
    {
      path: RoutePaths.Signup,
      name: Routes.CreatePark,
      component: () => import('../views/SignupView.vue'),
    },
    {
      path: RoutePaths.CreatePark,
      name: Routes.CreatePark,
      component: () => import('../views/CreateParkView.vue'),
    },
  ],
})

export default router
