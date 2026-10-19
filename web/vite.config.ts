import { defineConfig, loadEnv } from 'vite'
import vue from '@vitejs/plugin-vue'
export default defineConfig(({ mode, command }) => {
  const env = loadEnv(mode, process.cwd(), '')
  const apiBaseUrl = process.env.API_BASE_URL || env.API_BASE_URL
  if (command === 'serve' && !process.env.VITEST && !apiBaseUrl) {
    throw new Error('API_BASE_URL est requis pour le proxy Vue. Démarrer avec scripts/start-demo.ps1 : Aspire fournit cette adresse.')
  }
  return {
    plugins: [vue()],
    server: {
      port: 53170,
      strictPort: true,
      proxy: apiBaseUrl ? { '/api': { target: apiBaseUrl, changeOrigin: true } } : undefined,
    },
  }
})

