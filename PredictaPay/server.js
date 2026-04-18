const express = require('express');
const cors = require('cors');

const app = express();
const PORT = process.env.PORT || 3001;
const DOTNET_API_BASE_URL = process.env.DOTNET_API_BASE_URL || 'http://localhost:5076';

app.use(cors());
app.use(express.json());

app.get('/health', (req, res) => {
  res.json({
    status: 'ok',
    service: 'PredictaPay Web Proxy',
    backend: DOTNET_API_BASE_URL
  });
});

async function proxyToDotNet(req, res) {
  const targetUrl = new URL(req.originalUrl, DOTNET_API_BASE_URL);

  const headers = new Headers();
  Object.entries(req.headers).forEach(([key, value]) => {
    if (typeof value !== 'undefined' && key.toLowerCase() !== 'host') {
      headers.set(key, Array.isArray(value) ? value.join(',') : value);
    }
  });

  const fetchOptions = {
    method: req.method,
    headers
  };

  if (!['GET', 'HEAD'].includes(req.method)) {
    fetchOptions.body = JSON.stringify(req.body ?? {});
  }

  try {
    const response = await fetch(targetUrl, fetchOptions);
    const contentType = response.headers.get('content-type') || '';

    res.status(response.status);
    response.headers.forEach((value, key) => {
      if (key.toLowerCase() !== 'transfer-encoding') {
        res.setHeader(key, value);
      }
    });

    if (contentType.includes('application/json')) {
      const data = await response.json();
      return res.json(data);
    }

    const text = await response.text();
    return res.send(text);
  } catch (error) {
    return res.status(502).json({
      message: 'Unable to reach the .NET backend.',
      backend: DOTNET_API_BASE_URL,
      details: error instanceof Error ? error.message : 'Unknown error'
    });
  }
}

app.use('/api', proxyToDotNet);

app.listen(PORT, () => {
  console.log(`PredictaPay web proxy running on http://localhost:${PORT}`);
  console.log(`Forwarding API calls to ${DOTNET_API_BASE_URL}`);
});