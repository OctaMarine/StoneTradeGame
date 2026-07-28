import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'
import { fileURLToPath } from 'url'
import path from 'path'

const __filename = fileURLToPath(import.meta.url)
const __dirname = path.dirname(__filename)

export default defineConfig({
  plugins: [react(), tailwindcss()],
  server: {
  port: 3333,
  host: '0.0.0.0',
  allowedHosts: true,
  proxy: {
    '/api': {
      target: 'http://192.168.0.142:5000', // <-- Твой IP
      changeOrigin: true,
      secure: false,
      configure: (proxy) => {
        proxy.on('error', (err) => console.log('❌ [VITE PROXY ERROR]', err));
        proxy.on('proxyReq', (proxyReq, req) => {
          console.log(`🔀 [VITE PROXY] ${req.method} ${req.url} → ${proxyReq.path}`);
        });
      }
    }
  }
},
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
      '@src': path.resolve(__dirname, './src'),
    },
  },
})