import { createRouter, createWebHistory } from 'vue-router';

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      component: () => import('@/views/AppMainView.vue')
    },
    {
      path: '/edit/:id',
      component: () => import('@/views/AppEditView.vue')
    },
    {
      path: '/add',
      component: () => import('@/views/AppAddView.vue')
    }
  ]
})

export default router;
