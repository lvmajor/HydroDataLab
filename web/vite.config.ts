import { defineConfig, loadEnv } from 'vite'
import vue from '@vitejs/plugin-vue'
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '')
  return { plugins: [vue()], server: { port: 53170, strictPort: true, proxy: { '/api': { target: env.API_BASE_URL || 'http://localhost:53171', changeOrigin: true } } } }
})

