import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  /* config options here */
  devServer: {
    allowedDevOrigins: "http://192.168.0.142:3333",
  },
};

export default nextConfig;