namespace DSTEd.Core.Klei.Games {
    class DSTM : KleiGame {
        public DSTM(DSTEd core) : base(core) {
            this.id = 245850;
            this.name = I18N.__("Mod Tools");

            this.AddTool("FMOD_DESIGNER", "mod_tools/FMOD_Designer/fmod_designer.exe");
            this.AddTool("SPRITER", "mod_tools/Spriter/Spriter.exe");
            this.AddTool("TILED", "mod_tools/Tiled/tiled.exe");
            this.AddTool("TEXTURE_VIEWER", "mod_tools/tools/bin/TextureViewer.exe");
             /*
             * @ToDo add scripts with specific arguments/parameters (see these secripts, check their help files & arguments)
             * compiler_scripts/image_build.py
             * compiler_scripts/properties.py
             * compiler_scripts/properties.pyc
             * compiler_scripts/zipanim.py
             * 
             * mod_tools/export.py
             * mod_tools/ExportOptions.py
             * mod_tools/ResizeInfo.py
             * mod_tools/validate.py
             * 
             * scripts/ds_to_spriter.py
             * scripts/resize.py
             * 
             * tools/bin/ShaderCompiler.exe
             * tools/bin/TextureConverter.exe
             */
        }
    }
}
