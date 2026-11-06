using System.Net;
using System.Text;
using System.Globalization;
record Tile(int X,int Y,double Height,string Biome,string Life);
class Program {
 static Tile[,] Generate(int seed){
  var random=new Random(seed); var tiles=new Tile[72,36];
  double a=random.NextDouble()*6,b=random.NextDouble()*6,c=random.NextDouble()*6;
  for(int y=0;y<36;y++)for(int x=0;x<72;x++){
   double lon=x*Math.PI/36,lat=(y-18)*Math.PI/36;
   double h=Math.Sin(lon*3+a)*Math.Cos(lat*4+b)*.30+Math.Cos(lon*7+c)*Math.Cos(lat*6)*.12+Math.Sin(lat*9+a)*.12;
   double warmth=Math.Cos(lat)-Math.Max(h,0)*.6;
   string biome=h<-.08?"ocean":warmth<.35?"ice":h>.3?"mountain":warmth>.8&&Math.Sin(lon+a)>.3?"desert":"forest";
   tiles[x,y]=new(x,y,h,biome,biome switch{"ocean"=>"glowing plankton","forest"=>"fern grazers","desert"=>"burrowing beetles","ice"=>"crystal lichens",_=>"cloud moss"});
  }return tiles;
 }
 static string Svg(Tile[,] tiles){
  var s=new StringBuilder("<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 720 360' role='img' aria-label='Generated planet terrain map'>");
  var colors=new Dictionary<string,string>{{"ocean","#224e91"},{"ice","#d5eff3"},{"mountain","#8f8b9c"},{"desert","#d5ab63"},{"forest","#3b986c"}};
  foreach(var t in tiles)s.Append($"<rect x='{t.X*10}' y='{t.Y*10}' width='10' height='10' fill='{colors[t.Biome]}'><title>{t.Biome}: {t.Life}</title></rect>");
  return s.Append("</svg>").ToString();
 }
 static int Main(string[] args){
  if(args.Contains("--self-test")){var a=Generate(42);var b=Generate(42);if(Svg(a)!=Svg(b)||a.Length!=2592||Svg(a)==Svg(Generate(43)))return 1;Console.WriteLine("Planet generation tests passed");return 0;}
  int seed=42;var si=Array.IndexOf(args,"--seed");if(si>=0&&(si+1>=args.Length||!int.TryParse(args[si+1],out seed))){Console.Error.WriteLine("--seed requires an integer");return 2;}
  var tiles=Generate(seed);var svg=Svg(tiles);var export=Array.IndexOf(args,"--export");
  if(export>=0){if(export+1>=args.Length)return 2;File.WriteAllText(args[export+1],svg);return 0;}
  using var listener=new HttpListener();listener.Prefixes.Add("http://localhost:5080/");listener.Start();Console.WriteLine("Planet explorer: http://localhost:5080 — Ctrl+C to stop");
  while(true){var ctx=listener.GetContext();try{
   string path=ctx.Request.Url!.AbsolutePath;string text;string mime;
   if(path=="/planet.svg"){text=svg;mime="image/svg+xml";}
   else if(path=="/tile"){
    if(!int.TryParse(ctx.Request.QueryString["x"],out var x)||!int.TryParse(ctx.Request.QueryString["y"],out var y)||x<0||x>=72||y<0||y>=36){ctx.Response.StatusCode=400;text="Invalid coordinates";}
    else{var t=tiles[x,y];string weather=t.Biome=="desert"?"dust storm":t.Biome=="ice"?"snowfall":t.Biome=="ocean"?"sea fog":"gentle rain";text=$"Sector {x},{y} | {t.Biome} | elevation {t.Height.ToString("F2",CultureInfo.InvariantCulture)} | {weather} | native life: {t.Life}";}mime="text/plain";
   }else{text="""
<!doctype html><meta charset="utf-8"><meta name="viewport" content="width=device-width"><title>Planet Explorer</title>
<style>body{background:#0c1824;color:#d6f4e9;font:18px system-ui;max-width:1000px;margin:40px auto;padding:20px}img{width:100%;border-radius:20px;cursor:crosshair}h1{font-size:48px}button,a{color:#70e9b2}#info{min-height:60px;padding:20px;background:#173043}small{color:#90a7bc}</style>
<small>PROCEDURAL EXPLORATION</small><h1>A world from a number.</h1><p>Click any sector to discover its weather and native life.</p><img id="map" src="/planet.svg" alt="Planet terrain map"><p id="info" role="status">Select a sector.</p><a href="/planet.svg" download="planet.svg">Download map</a>
<script>document.getElementById('map').onclick=async e=>{const r=e.target.getBoundingClientRect();const x=Math.min(71,Math.floor((e.clientX-r.left)/r.width*72)),y=Math.min(35,Math.floor((e.clientY-r.top)/r.height*36));document.getElementById('info').textContent=await(await fetch(`/tile?x=${x}&y=${y}`)).text();};</script>
""";mime="text/html";}
   byte[] bytes=Encoding.UTF8.GetBytes(text);ctx.Response.ContentType=mime+"; charset=utf-8";ctx.Response.OutputStream.Write(bytes);
  }finally{ctx.Response.Close();}}
 }
}
