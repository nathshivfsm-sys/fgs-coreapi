# Assets

- **Owner:** AssetService (`asset`)
- **Purpose:** Customer/site assets, types, warranties, attributes
- **APIs:** `/api/v1/asset` and related attribute/type controllers
- **AuthZ:** `ASSET.*`
- **Clone:** `AssetController` + `CreateFgsAsset` feature set
- **Controllers:** mostly `ControllerBase` + StatusCode pattern
