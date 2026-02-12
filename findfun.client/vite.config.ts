import { fileURLToPath, URL } from 'node:url'
import fs from 'node:fs'
import path from 'node:path'
import child_process from 'node:child_process'
import { env } from 'node:process'

import AutoImport from 'unplugin-auto-import/vite'
import Components from 'unplugin-vue-components/vite'
import tailwindcss from '@tailwindcss/vite'
import { VitePWA } from 'vite-plugin-pwa'
import { ViteImageOptimizer } from 'vite-plugin-image-optimizer'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'
import { PrimeVueResolver } from '@primevue/auto-import-resolver'
import Icons from 'unplugin-icons/vite'
import IconResolver from 'unplugin-icons/resolver'

const baseFolder =
  env.APPDATA !== undefined && env.APPDATA !== ''
    ? `${env.APPDATA}/ASP.NET/https`
    : `${env.HOME}/.aspnet/https`

const certificateName = 'findfun.client'
const certFilePath = path.join(baseFolder, `${certificateName}.pem`)
const keyFilePath = path.join(baseFolder, `${certificateName}.key`)

if (!fs.existsSync(baseFolder)) {
  fs.mkdirSync(baseFolder, { recursive: true })
}

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
  if (
    child_process.spawnSync(
      'dotnet',
      ['dev-certs', 'https', '--export-path', certFilePath, '--format', 'Pem', '--no-password'],
      { stdio: 'inherit' }
    ).status !== 0
  ) {
    throw new Error('Could not create certificate.')
  }
}

const apiTarget = env['services__findfun-server__https__0'] ?? 'https://localhost:7117'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    tailwindcss(),
    vue(),
    vueDevTools(),
    AutoImport({
      imports: ['vue', 'vue-router', 'pinia'],
      dts: 'src/auto-imports.d.ts',
    }),
    Icons({ autoInstall: true }),
    Components({
      dirs: ['src/components'],
      extensions: ['vue'],
      deep: true,
      dts: 'src/components.d.ts',
      resolvers: [PrimeVueResolver(), IconResolver({ prefix: false })],
    }),
    VitePWA({
      registerType: 'autoUpdate',
      injectRegister: 'auto',
      manifest: {
        name: 'Swings & Slides Park',
        short_name: 'Swings & Slides',
        description: 'Discover and explore local parks with ratings, reviews, and real-time information',
        theme_color: '#1f2937',
        background_color: '#ffffff',
        display: 'standalone',
        orientation: 'portrait-primary',
        scope: '/',
        start_url: '/',
        icons: [
          { src: '/logo.png', sizes: '192x192', type: 'image/png', purpose: 'any' },
          { src: '/logo.png', sizes: '512x512', type: 'image/png', purpose: 'any' },
          { src: '/logo.png', sizes: '192x192', type: 'image/png', purpose: 'maskable' },
          { src: '/logo.png', sizes: '512x512', type: 'image/png', purpose: 'maskable' },
        ],
      },
      workbox: {
        globPatterns: ['**/*.{js,css,html,ico,png,svg,woff,woff2}'],
        runtimeCaching: [
          {
            urlPattern: /^https:\/\/api\..*/i,
            handler: 'StaleWhileRevalidate',
            options: {
              cacheName: 'api-cache',
              expiration: { maxEntries: 50, maxAgeSeconds: 24 * 60 * 60 },
            },
          },
          {
            urlPattern: /^https:\/\/.*\.(jpg|jpeg|png|gif|webp)$/i,
            handler: 'CacheFirst',
            options: {
              cacheName: 'images',
              expiration: { maxEntries: 60, maxAgeSeconds: 30 * 24 * 60 * 60 },
            },
          },
        ],
        navigateFallback: '/index.html',
        skipWaiting: true,
        clientsClaim: true,
      },
      devOptions: {
        enabled: true,
        suppressWarnings: true,
        navigateFallbackAllowlist: [/^(?!.*\.ts$).*$/],
      },
    }),
    ViteImageOptimizer({
      png: { quality: 75, compressionLevel: 9 },
      jpeg: { quality: 75, progressive: true },
      jpg: { quality: 75, progressive: true },
      webp: { quality: 75, lossless: false },
    }),
  ],

  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url)),
    },
  },

  build: {
    target: 'esnext',
    minify: 'terser',
    terserOptions: {
      compress: {
        drop_console: true,
        drop_debugger: true,
        pure_funcs: ['console.log', 'console.info', 'console.debug', 'console.warn'],
        passes: 3,
        ecma: 2020,
        module: true,
        arrows: true,
        arguments: true,
        booleans: true,
        collapse_vars: true,
        comparisons: true,
        computed_props: true,
        conditionals: true,
        dead_code: true,
        directives: true,
        evaluate: true,
        hoist_funs: true,
        hoist_props: true,
        hoist_vars: false,
        if_return: true,
        inline: 3,
        join_vars: true,
        keep_fargs: false,
        loops: true,
        negate_iife: true,
        properties: true,
        reduce_funcs: true,
        reduce_vars: true,
        sequences: true,
        side_effects: true,
        switches: true,
        toplevel: true,
        typeofs: true,
        unused: true,
      },
      mangle: {
        toplevel: true,
        safari10: true,
        properties: { regex: /^_/ },
      },
      format: { comments: false, ecma: 2020 },
    },
    cssMinify: 'lightningcss',
    rollupOptions: {
      output: {
        manualChunks: (id) => {
          if (id.includes('node_modules')) {
            if (id.includes('@vue/runtime-dom')) return 'vue-runtime-dom'
            if (id.includes('@vue/runtime-core')) return 'vue-runtime-core'
            if (id.includes('@vue/reactivity')) return 'vue-reactivity'
            if (id.includes('vue/') && !id.includes('vue-router')) return 'vue-core'
            if (id.includes('vue-router')) return 'vue-router'
            if (id.includes('pinia')) return 'pinia'
            if (id.includes('primevue/')) {
              if (id.includes('/button/') || id.includes('/inputtext/') || id.includes('/textarea/')) return 'primevue-basic'
              if (id.includes('/dialog/') || id.includes('/sidebar/') || id.includes('/toast/')) return 'primevue-overlay'
              if (id.includes('/datatable/') || id.includes('/column/') || id.includes('/paginator/')) return 'primevue-table'
              if (id.includes('/calendar/') || id.includes('/dropdown/') || id.includes('/multiselect/')) return 'primevue-form'
              return 'primevue-other'
            }
            if (id.includes('@primeuix/themes')) return 'primevue-theme'
            if (id.includes('@iconify') || id.includes('primeicons')) return 'icons'
            if (id.includes('axios')) return 'axios'
            if (id.includes('workbox')) return 'workbox'
            return 'vendor-other'
          }
        },
        chunkFileNames: 'assets/js/[name]-[hash].js',
        entryFileNames: 'assets/js/[name]-[hash].js',
        assetFileNames: (assetInfo) => {
          const info = assetInfo.name?.split('.') || []
          const ext = info[info.length - 1]
          if (/png|jpe?g|svg|gif|tiff|bmp|ico|webp/i.test(ext)) {
            return 'assets/images/[name]-[hash][extname]'
          }
          if (/woff|woff2|eot|ttf|otf/i.test(ext)) {
            return 'assets/fonts/[name]-[hash][extname]'
          }
          return 'assets/[name]-[hash][extname]'
        },
      },
    },
    reportCompressedSize: false,
    chunkSizeWarningLimit: 500,
  },

  server: {
    host: true,
    port: Number(env.PORT ?? env.DEV_SERVER_PORT ?? 62532),
    https: {
      key: fs.readFileSync(keyFilePath),
      cert: fs.readFileSync(certFilePath),
    },
    proxy: {
      '/api': {
        target: apiTarget,
        secure: false,
      },
      '/ipapi': {
        target: 'https://ipapi.co',
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/ipapi/, ''),
        secure: true,
      },
    },
  },
})
