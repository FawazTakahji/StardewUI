<lane orientation="vertical" horizontal-content-alignment="middle">
    <banner background={@Mods/StardewUI/Sprites/BannerBackground} background-border-thickness="48,0" padding="12" text="Settings" />
    <frame layout="500px content" background={@Mods/StardewUI/Sprites/ControlBorder} margin="0,8,0,0" padding="32,24">
        <lane layout="stretch content" orientation="vertical">
            <label font="dialogue" margin="0,0,0,8" text="Speed" />
            <lane layout="stretch content" margin="16,0" vertical-content-alignment="middle">
                <label layout="280px content" margin="0,8" text="Enable turbo boost" />
                <checkbox is-checked={<>EnableTurboBoost} />
            </lane>
            <lane layout="stretch content" margin="16,0" vertical-content-alignment="middle">
                <label layout="280px content" margin="0,8" text="Speed multiplier" />
                <slider min="10" max="100" value={<>SpeedMultiplier} />
            </lane>
        </lane>
    </frame>
</lane>