import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Default',
    component: () => import('../src/App.vue')
  },
  {
    path: '/WaitingRoom',
    name: 'WaitingRoom',
    component: () => import('../src/components/WaitingRoom.vue')
  }
//   {
//     path: '/home',
//     name: 'Home',
//     component: () => import('../components/Home.vue')
//   },
//   {
//     path: '/buy',
//     name: 'Buy',
//     component: () => import('../components/Buy.vue')
//   },
//   {
//     path: '/bucket',
//     name: 'Bucket',
//     component: () => import('../components/Bucket.vue')
//   }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;